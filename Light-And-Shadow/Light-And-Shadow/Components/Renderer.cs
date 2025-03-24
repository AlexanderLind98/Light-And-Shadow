using Light_And_Shadow.Worlds;
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

        public void Draw(Matrix4 mvp, Matrix4 model, Vector3 cameraPosition, int currentDebugMode, World currentWorld)
        {
            Material.UseShader();
            Material.SetUniform("mvp", mvp);
            Material.SetUniform("model", model);
            
            Matrix4 normalMatrix = Matrix4.Invert(model);

            Material.SetUniform("normalMatrix", normalMatrix); // Correct normal matrix passed to the shader

            // Set the transformed light direction
            Material.SetUniform("light.direction", Vector3.Normalize(currentWorld.SunDirection));
            Material.SetUniform("light.position", new Vector3(0, 5, 0));
            Material.SetUniform("light.constant", 1.0f);
            Material.SetUniform("light.linear", 0.09f);
            Material.SetUniform("light.quadratic", 0.032f);

            Material.SetUniform("viewPos", cameraPosition);

            Material.SetUniform("debugMode", currentDebugMode);
            
            Mesh.Draw();
        }
    }
}