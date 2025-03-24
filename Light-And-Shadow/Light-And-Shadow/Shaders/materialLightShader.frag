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
    vec3 direction;
    
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
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

    // Final color
    FragColor = vec4(result, 1.0);
//    FragColor = vec4(lightDir * 0.5 + 0.5, 1.0);

    // tester normalen
//    FragColor = vec4(normalize(Normal) * 0.5 + 0.5, 1.0);
//    FragColor = vec4(norm * 0.5 + 0.5, 1.0);
}
