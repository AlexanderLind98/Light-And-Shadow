using OpenTK_OBJ;
using OpenTK.Mathematics;

namespace Light_And_Shadow.Materials;

public class mat_box : Material
{
    public mat_box() : base()
    {
        uniforms.Add("material.diffTex", new Texture("Textures/Box.png"));
        uniforms.Add("material.specTex", new Texture("Textures/Box_specular.png"));
        uniforms.Add("material.ambient", new Vector3(0.25f, 0.25f, 0.25f));
        uniforms.Add("material.diffuse", new Vector3(1f));
        uniforms.Add("material.specular", new Vector3(10));
        uniforms.Add("material.shininess", 57.0f);
        
        UpdateUniforms();
    }
}

