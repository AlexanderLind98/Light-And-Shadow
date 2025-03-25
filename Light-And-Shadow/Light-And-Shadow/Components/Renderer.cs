using Light_And_Shadow.Worlds;
using OpenTK_OBJ;
using OpenTK.Graphics.OpenGL;
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

        public void Draw(Matrix4 mvp, Matrix4 model, Camera camera, int currentDebugMode, World currentWorld)
        {
            Material.UseShader();
            Material.SetUniform("mvp", mvp);
            Material.SetUniform("model", model);
            
            Matrix4 normalMatrix = Matrix4.Invert(model);

            Material.SetUniform("normalMatrix", normalMatrix); // Correct normal matrix passed to the shader

            // Set the transformed light direction
            Material.SetUniform("light.position", camera.Position);
            Material.SetUniform("light.direction", Vector3.Normalize(camera.Front));
            
            Console.WriteLine($"Camera pos: {camera.Position}");

            Material.SetUniform("light.cutOff", (float)MathHelper.Cos(MathHelper.DegreesToRadians(12.5f))); //12,5° radius
            Material.SetUniform("light.outerCutOff", (float)MathHelper.Cos(MathHelper.DegreesToRadians(30.5f))); //12,5° radius
            Material.SetUniform("viewPos", (Vector3)camera.Position);
            
            Material.SetUniform("light.constant", 1.0f);
            Material.SetUniform("light.linear", 0.09f);
            Material.SetUniform("light.quadratic", 0.032f);

            Material.SetUniform("debugMode", currentDebugMode);
            
            Mesh.Draw();
        }
    }
}