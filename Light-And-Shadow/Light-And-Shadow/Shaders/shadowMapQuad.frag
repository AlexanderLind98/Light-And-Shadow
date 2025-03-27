// Fragment Shader: shadowMapQuad.frag
#version 460 core

in vec2 TexCoords;
out vec4 FragColor;

uniform sampler2D shadowMap;

void main()
{
    float depthValue = texture(shadowMap, TexCoords).r;
    FragColor = vec4(vec3(1.0 - depthValue), 1.0); // Invert for clarity

}
