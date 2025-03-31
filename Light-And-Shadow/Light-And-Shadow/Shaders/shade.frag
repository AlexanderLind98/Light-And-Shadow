#version 460 core

uniform int debugMode;

struct Material {
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    float shininess;
};

struct DirLight {
    vec3 direction;
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

in vec3 Normal;
in vec3 FragPos;
in vec4 FragPosLightSpace; 

out vec4 FragColor;

uniform vec3 viewPos;
uniform Material material;
uniform DirLight dirLight;
uniform sampler2D shadowMap; 

float ShadowCalculation(vec4 fragPosLightSpace)
{
    vec3 projCoords = fragPosLightSpace.xyz / fragPosLightSpace.w;
    projCoords = projCoords * 0.5 + 0.5;

    if(projCoords.z > 1.0)
    return 0.0;

    float closestDepth = texture(shadowMap, projCoords.xy).r;
    float currentDepth = projCoords.z;

    float bias = 0.005;
    return currentDepth - bias > closestDepth ? 1.0 : 0.0;
}

void main()
{
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(-dirLight.direction);
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);

    float diff = max(dot(norm, lightDir), 0.0);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);

    vec3 ambient  = dirLight.ambient  * material.ambient;
    vec3 diffuse  = dirLight.diffuse  * diff * material.diffuse;
    vec3 specular = dirLight.specular * spec * material.specular;

  
    float shadow = ShadowCalculation(FragPosLightSpace);
    vec3 result = ambient + (1.0 - shadow) * (diffuse + specular);

    if (debugMode == 1)
    result = ambient;
    else if (debugMode == 2)
    result = diffuse;
    else if (debugMode == 3)
    result = specular;

    // === SHADOW DEBUG START ===
    FragColor = vec4(vec3(1.0 - shadow), 1.0); // Hvid = lys, sort = skygge
    return;
    // === SHADOW DEBUG END ===

    FragColor = vec4(result, 1.0);
}