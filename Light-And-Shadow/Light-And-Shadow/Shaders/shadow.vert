#version 460 core

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout(location = 2) in vec2 aTexCoord;

out vec2 TexCoords;
out vec3 Normal;
out vec3 FragPos;
out vec4 FragPosLightSpace;

uniform mat4 model;
uniform mat4 viewProjection;
uniform mat4 lightSpaceMatrix;

void main()
{
    // Beregn verdenspositionen for fragmentet ved at transformere det lokale vertex (aPos) med objektets model-matrix.
    FragPos = vec3(vec4(aPos, 1.0) * model);

    // Transformer normalvektoren med inverse-transponerede modelmatrix for at sikre korrekt orientering selv ved skalering og rotation.
    Normal = aNormal * mat3(transpose(inverse(model)));

    // Beregn fragmentets position i lysets koordinatsystem — bruges til skyggeberegning i fragment shader.
    FragPosLightSpace = vec4(FragPos, 1.0) * lightSpaceMatrix;

    // Beregn endelig position i clip-space: model * viewProjection * vertex.
    // Rækkefølgen svarer til CPU-siden, hvor vi bygger matricer som 'projection * view * model',
    // men da vi uploader med transpose=true, gør vi det fra venstre mod højre her.
    gl_Position = vec4(aPos, 1.0) * model * viewProjection;
}

