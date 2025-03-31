using OpenTK_OBJ;
using OpenTK.Mathematics;

namespace Light_And_Shadow.Materials;

public class mat_gold : Material
{
    public mat_gold() 
        : base("Shaders/shade.vert", "Shaders/shade.frag")
    {
        uniforms.Add("material.ambient", new Vector3(0.25f, 0.2f, 0.0745f));
        uniforms.Add("material.diffuse", new Vector3(0.75f, 0.6f, 0.2f));
        uniforms.Add("material.specular", new Vector3(0.625f, 0.55f, 0.35f));
        uniforms.Add("material.shininess", 51.2f);
        
        uniforms.Add("light.ambient", new Vector3(1.0f, 1.0f, 1.0f));
        uniforms.Add("light.diffuse", new Vector3(1.0f, 1.0f, 1.0f));
        uniforms.Add("light.specular", new Vector3(1.0f, 1.0f, 1.0f));
        
        UpdateUniforms();
    }
}

