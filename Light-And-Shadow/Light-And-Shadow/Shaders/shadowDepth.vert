// shadowDepth.vert
#version 460 core
layout(location = 0) in vec3 aPos;

uniform mat4 model;
uniform mat4 lightSpaceMatrix;

void main()
{
    // Transformér vertex-position fra objektets lokale rum til lysets clip-space.
    // Bruges i første render-pass til at gemme dybdeinformation i shadow map.
    //
    // Rækkefølgen matcher CPU-side matrixopbygning:
    // projection * view * model → uploadet med transpose=true →
    // derfor kan vi skrive det som vec4 * model * lightSpaceMatrix.
    gl_Position = vec4(aPos, 1.0) * model * lightSpaceMatrix;
}
