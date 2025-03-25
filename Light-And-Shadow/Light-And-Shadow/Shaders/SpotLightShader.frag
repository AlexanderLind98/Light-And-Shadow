#version 460 core

// debug mode input 
uniform int debugMode; // 0 = full, 1 = ambient, 2 = diffuse, 3 = specular

struct Material
{
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    float shininess;
};

struct Light
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

in vec3 Normal;
in vec3 FragPos;

out vec4 FragColor;

uniform vec3 viewPos;

uniform Material material;
uniform Light light;

void main()
{
    // Normalize input
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(-light.direction);
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);

    // Diffuse
    float diff = max(dot(norm, lightDir), 0.0);
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


//    FragColor = vec4(light.position, 1.0);

/**    // Smooth spotlight effect
    float epsilon = 0.1;  // Soft transition range
    float intensity = smoothstep(light.cutOff - epsilon, light.cutOff, theta);*/

    // Apply intensity to lighting result
    vec3 finalColor = (ambient + diffuse + specular) * intensity + ambient;
    FragColor = vec4(finalColor, 1.0);

}
