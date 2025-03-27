#version 460 core

layout (location = 0) in vec3 aPos; 
layout(location = 1) in vec2 aTexCoord;
layout (location = 2) in vec3 aNormal;

out VS_Out
{
    vec3 FragPos;
    vec3 Normal;
    vec2 TexCoords;
    vec4 FragPosLightSpace;
} vs_out;

uniform mat4 mvp;
uniform mat4 model;
//uniform mat4 normalMatrix;  // To correctly transform normals to world space
uniform mat4 lightSpaceMatrix;

void main()
{
    vs_out.FragPos = vec3(model * vec4(aPos, 1.0));
//    vs_out.FragPos = vec3(vec4(aPos, 1.0) * model);
    vs_out.Normal = inverse(mat3(model)) * aNormal;
    vs_out.TexCoords = aTexCoord;
    vs_out.FragPosLightSpace = lightSpaceMatrix * vec4(vs_out.FragPos, 1.0);
    
    gl_Position = vec4(vs_out.FragPos, 1.0) * mvp;
}