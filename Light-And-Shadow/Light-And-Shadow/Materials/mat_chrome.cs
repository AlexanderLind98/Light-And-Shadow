using OpenTK_OBJ;
using OpenTK.Mathematics;

namespace Light_And_Shadow.Materials;

public class mat_chrome : Material
{
    public mat_chrome() : base()
    {
        uniforms.Add("material.diffTex", new Texture("Textures/scratches.jpg"));
        uniforms.Add("material.ambient", new Vector3(0.4f));
        uniforms.Add("material.diffuse", new Vector3(0.6f));
        uniforms.Add("material.specular", new Vector3(0.774f));
        uniforms.Add("material.shininess", 96.8f);
        
        UpdateUniforms();
    }
}