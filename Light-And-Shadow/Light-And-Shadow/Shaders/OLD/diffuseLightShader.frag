#version 460 core

flat in vec3 Normal;
in vec3 FragPos;

out vec4 FragColor;

uniform vec3 lightPos;
uniform vec3 lightColor;
uniform float ambientStrength;
uniform vec3 objectColor;

void main()
{
    vec3 norm = normalize(Normal);
    
    // Derivatives for normal mapping
    //vec3 norm = normalize(cross(dFdx(FragPos), dFdy(FragPos)));


    vec3 lightDir = normalize(lightPos - FragPos);

    vec3 ambient = ambientStrength * lightColor;

    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = diff * lightColor;

    vec3 result = (ambient + diffuse) * objectColor;
    FragColor = vec4(result, 1.0);

    // tester normalen
    //FragColor = vec4(normalize(Normal) * 0.5 + 0.5, 1.0);
    //FragColor = vec4(norm * 0.5 + 0.5, 1.0);

}
