using OpenTK_OBJ;
using OpenTK.Mathematics;

namespace Light_And_Shadow.Materials;

public class mat_concrete : Material
{
    public mat_concrete() : base()
    {
        uniforms.Add("material.ambient", new Vector3(0.25f, 0.25f, 0.25f));
        uniforms.Add("material.diffuse", new Vector3(0.25f, 0.25f, 0.25f));
        uniforms.Add("material.specular", new Vector3(0.25f, 0.25f, 0.25f));
        uniforms.Add("material.shininess", 10.0f);
        
        uniforms.Add("light.ambient", new Vector3(1.0f, 1.0f, 1.0f));
        uniforms.Add("light.diffuse", new Vector3(1.0f, 1.0f, 1.0f));
        uniforms.Add("light.specular", new Vector3(1.0f, 1.0f, 1.0f));
        
        UpdateUniforms();
    }
}

