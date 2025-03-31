#version 460 core

// debug mode input 
uniform int debugMode; // 0 = full, 1 = ambient, 2 = diffuse, 3 = specular

//Structs
struct Material
{
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    float shininess;
};

struct DirLight
{
    vec3 direction;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

struct PointLight
{
    vec3 position;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float constant;
    float linear;
    float quadratic;
};

struct SpotLight
{
    vec3 position;
    vec3 direction;
    float cutOff;
    float outerCutOff;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float constant;
    float linear;
    float quadratic;
};

//Inputs
in VS_Out
{
    vec3 FragPos;
    vec3 Normal;
    vec2 TexCoords;
    vec4 FragPosLightSpace;
} fs_in;

//Outputs
out vec4 FragColor;

//Defines
#define MAX_POINTLIGHTS 16
#define MAX_SPOTLIGHTS 16

//Uniforms
uniform sampler2D shadowMap;
uniform vec3 viewPos;
uniform Material material;
uniform DirLight dirLight;
uniform int numPointLights;
uniform int numSpotLights;
uniform PointLight pointLights[MAX_POINTLIGHTS];
uniform SpotLight spotLights[MAX_SPOTLIGHTS];

//Prototypes / definitions
float ShadowCalculation(vec4 fragPosLightSpace);
vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir);
vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir);
vec3 CalcSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir);

float ShadowCalculation(vec4 fragPosLightSpace)
{
    
    // perform perspective divide
    vec3 projCoords = fragPosLightSpace.xyz / fragPosLightSpace.w;
    // transform to [0,1] range
    projCoords = projCoords * 0.5 + 0.5;

    // get depth of current fragment from light's perspective
    if (projCoords.z > 1.0)
    return 0.0;
    
    // get closest depth value from light's perspective (using [0,1] range fragPosLight as coords)
    float closestDepth = texture(shadowMap, projCoords.xy).r;
    // get depth of current fragment from light's perspective
    float currentDepth = projCoords.z;
    // check whether current frag pos is in shadow
    float bias = max(0.005 * (1.0 - dot(normalize(fs_in.Normal), normalize(dirLight.direction))), 0.0005);
    
    // Kinda PCF
    float shadow = currentDepth - bias > closestDepth ? 1.0 : 0.0;

    return shadow;
}

//Methods
vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir)
{
    vec3 lightDir = normalize(-light.direction);
    vec3 halfwayDir = normalize(lightDir + viewDir);
    const float pi = 3.14159265;
    
    // diffuse shading
    float diff = max(dot(normal, lightDir), 0.0);
    
    // specular shading
    vec3 reflectDir = reflect(-lightDir, normal);
    const float energyConservation = (50.0 + material.shininess) / (50.0 * pi);
    float spec = energyConservation * pow(max(dot(normal, halfwayDir), 0.0f), material.shininess);
    
    // combine results
    vec3 ambient  = light.ambient  * material.ambient;
    vec3 diffuse  = light.diffuse  * diff * material.diffuse;
    vec3 specular = light.specular * spec;

    float shadow = ShadowCalculation(fs_in.FragPosLightSpace);
    vec3 lighting = (ambient + (1.0 - shadow) * (diffuse + specular)) * material.diffuse;

    // Final lighting based on debug mode
    vec3 result;
    if (debugMode == 1)
    result = ambient;
    else if (debugMode == 2)
    result = diffuse;
    else if (debugMode == 3)
    result = specular;
    else
//    result = (ambient + diffuse + specular);
    result = vec3(lighting);
//    result = vec3(1.0 - shadow);
    
    return result;    
}

vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
    vec3 lightDir = normalize(light.position - fragPos);
    vec3 halfwayDir = normalize(lightDir + viewDir);
    const float pi = 3.14159265;
    
    // diffuse shading
    float diff = max(dot(normal, lightDir), 0.0);
    
    // specular shading
    vec3 reflectDir = reflect(-lightDir, normal);
    const float energyConservation = (16.0 + material.shininess) / (16.0 * pi);
    float spec = energyConservation * pow(max(dot(normal, halfwayDir), 0.0f), material.shininess);
    
    // attenuation
    float distance    = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance +
    light.quadratic * (distance * distance));
    
    // combine results
    vec3 ambient  = light.ambient  * material.diffuse;
    vec3 diffuse  = light.diffuse  * diff * material.diffuse;
    vec3 specular = light.specular * spec;
    
    ambient  *= attenuation;
    diffuse  *= attenuation;
    specular *= attenuation;

    // Final lighting based on debug mode
    vec3 result;
    if (debugMode == 1)
    result = ambient;
    else if (debugMode == 2)
    result = diffuse;
    else if (debugMode == 3)
    result = specular;
    else
    result = (ambient + diffuse + specular);
    
    return result;
}

vec3 CalcSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
    // Normalize input
    vec3 lightDir = normalize(-light.direction);
    vec3 reflectDir = reflect(-lightDir, normal);
    vec3 halfwayDir = normalize(lightDir + viewDir);
    const float pi = 3.14159265;

    // Diffuse
    float diff = max(dot(normal, lightDir), 0.0);
    // Specular
//    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    const float energyConservation = (16.0 + material.shininess) / (16.0 * pi);
    float spec = energyConservation * pow(max(dot(normal, halfwayDir), 0.0f), material.shininess);
    
    // Ambient
    vec3 ambient = light.ambient * material.ambient;
    vec3 diffuse = light.diffuse * (diff * material.diffuse);
//    vec3 specular = light.specular * (spec * material.specular);
    vec3 specular = light.specular * spec;

    float distance    = length(light.position - fs_in.FragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));

    vec3 LightToPixel = normalize(light.position - fs_in.FragPos );

    float theta = dot(LightToPixel, normalize(-light.direction));
    float epsilon = 0.01;  // Soft transition range
    float intensity = smoothstep(light.cutOff - epsilon, light.cutOff, theta);

    ambient *= attenuation;
    diffuse *= intensity;
    specular *= intensity;

    // Final lighting based on debug mode
    vec3 result;
    if (debugMode == 1)
    result = ambient;
    else if (debugMode == 2)
    result = diffuse;
    else if (debugMode == 3)
    result = specular;
    else
    result = (ambient + diffuse + specular);

    // Apply intensity to lighting result
    vec3 finalColor = result * intensity + ambient;
    
    return finalColor;
}

void main()
{
    vec3 norm = normalize(fs_in.Normal);
    vec3 viewDir = normalize(viewPos - fs_in.FragPos);

    // === SHADOW DEBUG START ===
    float shadowDebug = ShadowCalculation(fs_in.FragPosLightSpace);
    FragColor = vec4(vec3(1.0 - shadowDebug), 1.0); // White = light, Black = shadow
    return;
    // === SHADOW DEBUG END ===

    vec3 result = CalcDirLight(dirLight, norm, viewDir);

    if (numPointLights != 0)
    {
        for (int i = 0; i < numPointLights; i++)
        {
            result += CalcPointLight(pointLights[i], norm, fs_in.FragPos, viewDir);
        }
    }

    if (numSpotLights != 0)
    {
        for (int i = 0; i < numSpotLights; i++)
        {
            result += CalcSpotLight(spotLights[i], norm, fs_in.FragPos, viewDir);
        }
    }

    FragColor = vec4(result, 1.0f);
}
