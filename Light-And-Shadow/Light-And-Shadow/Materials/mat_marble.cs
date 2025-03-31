using OpenTK_OBJ;
using OpenTK.Mathematics;

namespace Light_And_Shadow.Materials;

public class mat_marble : Material
{
    public mat_marble() : base()
    {
        uniforms.Add("material.ambient", new Vector3(0.25f, 0.25f, 0.25f));
        uniforms.Add("material.diffuse", new Vector3(0.75f, 0.75f, 0.75f));
        uniforms.Add("material.specular", new Vector3(0.1f, 0.1f, 0.1f));
        uniforms.Add("material.shininess", 10.0f);
        
        UpdateUniforms();
    }
}

