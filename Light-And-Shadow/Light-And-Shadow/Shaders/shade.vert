#version 460 core

layout (location = 0) in vec3 aPos;
layout (location = 2) in vec3 aNormal;

out vec3 Normal;
out vec3 FragPos;
out vec4 FragPosLightSpace; 

uniform mat4 mvp;
uniform mat4 model;
uniform mat4 lightSpaceMatrix; 
uniform mat4 normalMatrix;

void main()
{
    vec4 worldPos = vec4(aPos, 1.0) * model;

    gl_Position = vec4(aPos, 1.0) * mvp;
    FragPos = worldPos.xyz;
    Normal = normalize(mat3(normalMatrix) * aNormal);
    FragPosLightSpace = vec4(aPos, 1.0) * model * lightSpaceMatrix; 
}
