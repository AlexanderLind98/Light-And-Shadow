#version 460 core

// debug mode input 
uniform int debugMode; // 0 = full, 1 = ambient, 2 = diffuse, 3 = specular


flat in vec3 Normal;
in vec3 FragPos;

out vec4 FragColor;

uniform vec3 viewPos;
uniform vec3 lightPos;
uniform vec3 lightColor;
uniform float ambientStrength;
uniform vec3 objectColor;
uniform float shininess;
uniform float specularStrength;

// shadow
uniform sampler2D shadowMap;
uniform mat4 lightSpaceMatrix;


void main()
{
    // Normalize input
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(lightPos - FragPos);
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);

    // Ambient
    vec3 ambient = ambientStrength * lightColor;

    // Diffuse
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = diff * lightColor;

    // Specular
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
    vec3 specular = specularStrength * spec * lightColor;

//    // Final lighting based on debug mode
//    vec3 result;
//    if (debugMode == 1)
//    result = ambient;
//    else if (debugMode == 2)
//    result = diffuse;
//    else if (debugMode == 3)
//    result = specular;
//    else
//    result = (ambient + diffuse + specular);

    // Final color
    //FragColor = vec4(result * objectColor, 1.0);

    // Shadow mapping
    vec4 fragPosLightSpace = lightSpaceMatrix * vec4(FragPos, 1.0);
    vec3 projCoords = fragPosLightSpace.xyz / fragPosLightSpace.w;
    projCoords = projCoords * 0.5 + 0.5;

    // Check if in shadow
    float closestDepth = texture(shadowMap, projCoords.xy).r;
    float currentDepth = projCoords.z;
    //float bias = 0.005;
    float bias = max(0.05 * (1.0 - dot(norm, lightDir)), 0.005);

    float shadow = currentDepth > closestDepth + bias ? 1.0 : 0.0;

    // Final lighting with debug modes
    vec3 lighting;
    if (debugMode == 1)
    lighting = ambient;
    else if (debugMode == 2)
    lighting = (1.0 - shadow) * diffuse;
    else if (debugMode == 3)
    lighting = (1.0 - shadow) * specular;
    else
    lighting = ambient + (1.0 - shadow) * (diffuse + specular);

    FragColor = vec4(lighting * objectColor, 1.0);
}
