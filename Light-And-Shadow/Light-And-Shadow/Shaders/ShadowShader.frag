#version 460 core

in VS_Out
{
    vec3 FragPos;
    vec3 Normal;
    vec2 TexCoords;
    vec4 FragPosLightSpace;
} fs_in;

out vec4 FragColor;

uniform int debugMode;

uniform vec3 viewPos;
uniform vec3 lightPos;
uniform vec3 lightColor;
uniform float ambientStrength;
uniform vec3 objectColor;
uniform float shininess;
uniform float specularStrength;

uniform sampler2D shadowMap;

float ShadowCalculation(vec4 fragPosLightSpace)
{
    vec3 projCoords = fragPosLightSpace.xyz / fragPosLightSpace.w;
    projCoords = projCoords * 0.5 + 0.5;

    float closestDepth = texture(shadowMap, projCoords.xy).r;
    float currentDepth = projCoords.z;

    float bias = max(0.05 * (1.0 - dot(normalize(fs_in.Normal), normalize(lightPos - fs_in.FragPos))), 0.005);

    float shadow = currentDepth - bias > closestDepth ? 1.0 : 0.0;

    if(projCoords.z > 1.0)
    shadow = 0.0;

    return shadow;
}

void main()
{
    vec3 norm = normalize(fs_in.Normal);
    vec3 lightDir = normalize(lightPos - fs_in.FragPos);
    vec3 viewDir = normalize(viewPos - fs_in.FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);

    vec3 ambient = ambientStrength * lightColor;

    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = diff * lightColor;

    float spec = pow(max(dot(viewDir, reflectDir), 0.0), shininess);
    vec3 specular = specularStrength * spec * lightColor;

    float shadow = 0.0;

    if (debugMode == 4 || debugMode == 5)
    {
        shadow = ShadowCalculation(fs_in.FragPosLightSpace);
    }

    vec3 result;

    vec3 lightColor = vec3(1.0);
    vec3 lightPos = vec3(5.0, 10.0, 5.0);

    if (debugMode == 1)
    result = ambient;
    else if (debugMode == 2)
    result = diffuse;
    else if (debugMode == 3)
    result = specular;
    else if (debugMode == 4)
    result = ambient + (1.0 - shadow) * (diffuse + specular);
    else if (debugMode == 5)
    result = vec3(1.0 - shadow); // Visualize shadow factor
    else if (debugMode == 6)
    FragColor = vec4(diff, diff, diff, 1.0); // shows raw diffuse factor
    else if (debugMode == 7)
    FragColor = vec4(spec, spec, spec, 1.0); // shows raw specular intensity

    else
    result = ambient + diffuse + specular;

    FragColor = vec4(result * objectColor, 1.0);
    //FragColor = vec4(1.0 - shadow, 1.0 - shadow, 1.0 - shadow, 1.0); // test

}
