using OpenTK_OBJ;
using OpenTK.Mathematics;

namespace Light_And_Shadow.Materials;

public class mat_simple : Material
{
    public mat_simple() 
        : base("Shaders/OLD/ambientLightShader.vert", "Shaders/OLD/ambientLightShader.frag")
    {
        SetUniform("material.ambient", new Vector3(0.5f, 0.5f, 0.5f));
        SetUniform("material.diffuse", new Vector3(0.75f, 0.6f, 0.2f));
        SetUniform("material.specular", new Vector3(0.625f, 0.55f, 0.35f));
        SetUniform("material.shininess", 51.2f);

        SetUniform("light.ambient", new Vector3(1.0f));
        SetUniform("light.diffuse", new Vector3(1.0f));
        SetUniform("light.specular", new Vector3(1.0f));

        UpdateUniforms();
    }
}