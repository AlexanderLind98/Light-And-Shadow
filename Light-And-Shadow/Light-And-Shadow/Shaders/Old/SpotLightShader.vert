#version 460 core

layout (location = 0) in vec3 aPos;
layout (location = 2) in vec3 aNormal;

out vec3 Normal;
out vec3 FragPos;

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

uniform mat4 mvp;
uniform mat4 model;
uniform mat4 normalMatrix;  // To correctly transform normals to world space
uniform Light light;

void main()
{
    gl_Position = vec4(aPos, 1.0) * mvp;
    FragPos = vec3(vec4(aPos, 1.0) * model);
    Normal = normalize(mat3(normalMatrix) * aNormal);
}