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
in vec3 Normal;
in vec3 FragPos;
in vec2 texCoord;

//Outputs
out vec4 FragColor;

//Defines
#define MAX_POINTLIGHTS 16
#define MAX_SPOTLIGHTS 16

//Uniforms
uniform vec3 viewPos;
uniform Material material;
uniform DirLight dirLight;
uniform int numPointLights;
uniform int numSpotLights;
uniform PointLight pointLights[MAX_POINTLIGHTS];
uniform SpotLight spotLights[MAX_SPOTLIGHTS];

//Prototypes / definitions
vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir);
vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir);
vec3 CalcSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir);

//Methods
vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir)
{
    vec3 lightDir = normalize(-light.direction);
    
    // diffuse shading
    float diff = max(dot(normal, lightDir), 0.0);
    
    // specular shading
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    
    // combine results
    vec3 ambient  = light.ambient  * material.ambient;
    vec3 diffuse  = light.diffuse  * diff * material.diffuse;
    vec3 specular = light.specular * spec * material.specular;

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

vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
    vec3 lightDir = normalize(light.position - fragPos);
    
    // diffuse shading
    float diff = max(dot(normal, lightDir), 0.0);
    
    // specular shading
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    
    // attenuation
    float distance    = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance +
    light.quadratic * (distance * distance));
    
    // combine results
    vec3 ambient  = light.ambient  * material.diffuse;
    vec3 diffuse  = light.diffuse  * diff * material.diffuse;
    vec3 specular = light.specular * spec * material.specular;
    
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

    // Diffuse
    float diff = max(dot(normal, lightDir), 0.0);
    // Specular
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);

    // Ambient
    vec3 ambient = light.ambient * material.ambient;
    vec3 diffuse = light.diffuse * (diff * material.diffuse);
    vec3 specular = light.specular * (spec * material.specular);

    float distance    = length(light.position - FragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));

    vec3 LightToPixel = normalize(light.position - FragPos  );

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
    // Normalize input
    vec3 norm = normalize(Normal);
    vec3 viewDir = normalize(viewPos - FragPos);
    
    vec3 result;
    
    result = CalcDirLight(dirLight, norm, viewDir);
    
    if(numPointLights != 0) //Only calc lights if lights exist!
    {
         for (int i = 0; i < numPointLights; i++)
         {
             result += CalcPointLight(pointLights[i], norm, FragPos, viewDir);
         }
    }

    if(numSpotLights != 0) //Only calc lights if lights exist!
    {
        for(int i = 0; i < numSpotLights; i++)
        {
            result += CalcSpotLight(spotLights[i], norm, FragPos, viewDir);
        }
    }
    
    FragColor = vec4(result, 1.0f);
}
