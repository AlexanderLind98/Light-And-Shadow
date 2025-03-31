using OpenTK_OBJ;
using OpenTK.Mathematics;

namespace Light_And_Shadow.Materials;

public class mat_wall : Material
{
    public mat_wall() : base()
    {
        uniforms.Add("material.diffTex", new Texture("Textures/wall.jpg"));
        uniforms.Add("material.ambient", new Vector3(0.2f));
        uniforms.Add("material.diffuse", new Vector3(0.2f));
        uniforms.Add("material.specular", new Vector3(0.1f));
        uniforms.Add("material.shininess", 0.5f);

        UpdateUniforms();
    }
}

