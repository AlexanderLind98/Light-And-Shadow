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

    float bias = max(0.05 * (1.0 - dot(normalize(fs_in.Normal), normalize(-lightPos))), 0.005);

    float shadow = currentDepth - bias > closestDepth ? 1.0 : 0.0;

    if(projCoords.z > 1.0)
    shadow = 0.0;

    return shadow;
}

void main()
{
    vec3 norm = normalize(fs_in.Normal);
    vec3 lightDir = normalize(-lightPos); // match dirLight.direction style
    vec3 viewDir = normalize(viewPos - fs_in.FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);

    float diff = max(dot(norm, lightDir), 0.0);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), shininess);

    // Match the classic material × light Phong model
    vec3 ambient  = lightColor * objectColor * ambientStrength;
    vec3 diffuse  = lightColor * objectColor * diff;
    vec3 specular = lightColor * specularStrength * spec;

    float shadow = 0.0;
    if (debugMode == 4 || debugMode == 5)
    {
        shadow = ShadowCalculation(fs_in.FragPosLightSpace);
    }

    vec3 result;
    vec3 rawLighting = ambient + diffuse + specular;

    if (debugMode == 1)
    result = ambient;
    else if (debugMode == 2)
    result = diffuse;
    else if (debugMode == 3)
    result = specular;
    else if (debugMode == 4)
    result = ambient + (1.0 - shadow) * (diffuse + specular);
    else if (debugMode == 5)
    result = vec3(1.0 - shadow);
    else
    result = rawLighting;

    FragColor = vec4(result, 1.0);
}
