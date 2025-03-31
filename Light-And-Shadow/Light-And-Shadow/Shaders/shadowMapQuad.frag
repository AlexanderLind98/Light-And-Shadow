#version 460 core

out vec4 FragColor;
in vec2 TexCoords;

uniform sampler2D shadowMap;

void main()
{
    float d = texture(shadowMap, TexCoords).r;

    
    //FragColor = vec4(TexCoords, 0.0, 1.0); // Visualiser UVs
    


    FragColor = vec4(vec3(d), 1.0); 
    //FragColor = vec4(vec3(pow(d, 25.0)), 1.0);
    //FragColor = vec4(vec3(1.0 - d), 1.0); 

   

//    if (d >= 0.999)
//    FragColor = vec4(1.0, 0.0, 0.0, 1.0);
//    else if (d > 0.5)
//    FragColor = vec4(0.0, 1.0, 0.0, 1.0); 
//    else
//    FragColor = vec4(0.0, 0.0, 1.0, 1.0); 

}
