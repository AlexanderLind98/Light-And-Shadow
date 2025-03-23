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
    FragColor = vec4(result * objectColor, 1.0);

    // Final color
    //vec3 result = ambient + diffuse + specular;
    //vec3 result = (ambient + diffuse + specular) * objectColor;
    //FragColor = vec4(result, 1.0);

    // Tester specular
//    vec3 specular = spec * lightColor;
//    FragColor = vec4(specular, 1.0);
//    float rawDot = dot(viewDir, reflectDir);
//    FragColor = vec4(vec3(rawDot), 1.0);


    // tester normalen
    //FragColor = vec4(normalize(Normal) * 0.5 + 0.5, 1.0);
    //FragColor = vec4(norm * 0.5 + 0.5, 1.0);
}
