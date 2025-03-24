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

        public void Draw(Matrix4 mvp, Matrix4 model, Vector3 cameraPosition, int currentDebugMode)
        {
            Material.UseShader();
            Material.SetUniform("mvp", mvp);
            Material.SetUniform("model", model);
            Material.SetUniform("lightPos", new Vector3(0.0f, 2.0f, -3.0f));
            Material.SetUniform("viewPos", cameraPosition);
            Material.SetUniform("lightColor", Vector3.One);

            Material.SetUniform("debugMode", currentDebugMode);
            
            Mesh.Draw();
        }
    }
}