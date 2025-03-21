#version 460 core
layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTexCoord;
layout(location = 2) in vec3 aNormal;  

out vec2 texCoord;

uniform mat4 mvp;

void main()
{
    texCoord = aTexCoord;

    gl_Position = vec4(aPosition, 1.0)*mvp;
}
