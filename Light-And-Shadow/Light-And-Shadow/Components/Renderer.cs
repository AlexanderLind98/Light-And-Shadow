using OpenTK_OBJ;
using OpenTK.Mathematics;

namespace Light_And_Shadow.Components
{
    public class Renderer
    {
        public Material Material { get; set; }
        public Mesh Mesh { get; set; }

        public Renderer(Material material, Mesh mesh)
        {
            Material = material;
            Mesh = mesh;
        }

        public void Draw(Matrix4 mvp)
        {
            Material.UseShader();
            Material.SetUniform("mvp", mvp);
            Mesh.Draw();
        }
    }
}