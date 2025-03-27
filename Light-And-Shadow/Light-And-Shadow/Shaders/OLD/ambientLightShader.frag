#version 460 core

in vec3 objectColor;

out vec4 FragColor;

void main()
{
    const vec3 lightColor = vec3(1.0, 1.0, 1.0); 
    const float ambientStrength = 0.1;
    vec3 ambient = ambientStrength * lightColor;

    vec3 result = ambient * objectColor;
    FragColor = vec4(result, 1.0);
}
