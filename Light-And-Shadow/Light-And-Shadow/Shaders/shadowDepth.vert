// shadowDepth.vert
#version 460 core
layout(location = 0) in vec3 aPos;

uniform mat4 model;
uniform mat4 lightSpaceMatrix;

void main()
{
    // Transformer vertex-position fra objektets lokale rum til lysets clip-space.
    // Bruges i foerste render-pass til at gemme dybdeinformation i shadow map.
    gl_Position = vec4(aPos, 1.0) * model * lightSpaceMatrix;
}
