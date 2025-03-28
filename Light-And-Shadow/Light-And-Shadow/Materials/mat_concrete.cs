using OpenTK_OBJ;
using OpenTK.Mathematics;

namespace Light_And_Shadow.Materials;

public class mat_concrete : Material
{
    public mat_concrete() : base()
    {
        uniforms.Add("material.ambient", new Vector3(0.5f, 0.5f, 0.5f));
        uniforms.Add("material.diffuse", new Vector3(0.2f, 0.2f, 0.2f));
        uniforms.Add("material.specular", new Vector3(0.005f, 0.005f, 0.005f));
        uniforms.Add("material.shininess", 10.0f);
        
        uniforms.Add("light.ambient", new Vector3(0.10f, 0.10f, 0.10f));
        uniforms.Add("light.diffuse", new Vector3(0.10f, 0.10f, 0.10f));
        uniforms.Add("light.specular", new Vector3(0.01f, 0.01f, 0.01f));
        
        UpdateUniforms();
    }
}

