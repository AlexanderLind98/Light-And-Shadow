using OpenTK.Mathematics;

namespace Light_And_Shadow.Materials;

public class mat_default : Material
{
    public mat_default()
        : base("Shaders/BlinnPhongShader.vert", "Shaders/BlinnPhongShader.frag")
    {
        SetUniform("material.ambient", new Vector3(0.7f));
        SetUniform("material.diffuse", new Vector3(0.7f));
        SetUniform("material.specular", new Vector3(0.1f));
        SetUniform("material.shininess", 8f);

        UpdateUniforms();
    }
}
