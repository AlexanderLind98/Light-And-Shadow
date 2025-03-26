using OpenTK.Mathematics;

namespace Light_And_Shadow.Materials;

public class mat_shadow : Material
{
    public mat_shadow()
        : base("Shaders/depthShader.vert", "Shaders/depthShader.frag")
    {
        /*SetUniform("material.ambient", new Vector3(0.7f));
        SetUniform("material.diffuse", new Vector3(0.7f));
        SetUniform("material.specular", new Vector3(0.1f));
        SetUniform("material.shininess", 8f);

        SetUniform("light.ambient", new Vector3(1f));
        SetUniform("light.diffuse", new Vector3(1f));
        SetUniform("light.specular", new Vector3(1f));*/
        
        // SetUniform("depthMap", shader.depthMap);

        UpdateUniforms();
    }
}
