using OpenTK_OBJ;
using OpenTK.Mathematics;

namespace Light_And_Shadow.Materials;

public class mat_gold_simple : Material
{
    public mat_gold_simple() 
        : base("Shaders/BlinnPhongShader.vert", "Shaders/BlinnPhongShader.frag")
    {
        SetUniform("material.ambient", new Vector3(0.25f, 0.2f, 0.0745f));
        SetUniform("material.diffuse", new Vector3(0.75f, 0.6f, 0.2f));
        SetUniform("material.specular", new Vector3(0.625f, 0.55f, 0.35f));
        SetUniform("material.shininess", 51.2f);

        UpdateUniforms();
    }
}