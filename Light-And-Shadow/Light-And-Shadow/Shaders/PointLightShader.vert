#version 460 core

layout (location = 0) in vec3 aPos;
layout (location = 2) in vec3 aNormal;

out vec3 Normal;
out vec3 FragPos;

uniform mat4 mvp;
uniform mat4 model;
uniform mat4 normalMatrix;  // To correctly transform normals to world space

void main()
{
    gl_Position = vec4(aPos, 1.0) * mvp;
    FragPos = vec3(model * vec4(aPos, 1.0));
    Normal = normalize(mat3(normalMatrix) * aNormal);
}