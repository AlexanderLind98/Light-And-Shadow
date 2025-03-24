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
            
            // Transform light direction into object space
            Vector3 lightDirectionInObjectSpace = Vector3.TransformNormal(currentWorld.SunDirection, Matrix4.Invert(model));
            
            Matrix4 normalMatrix = Matrix4.Transpose(Matrix4.Invert(model));
            Material.SetUniform("normalMatrix", normalMatrix);

            // Set the transformed light direction
            Material.SetUniform("light.direction", Vector3.Normalize(lightDirectionInObjectSpace));

            Material.SetUniform("viewPos", cameraPosition);

            Material.SetUniform("debugMode", currentDebugMode);
            
            Mesh.Draw();
        }
    }
}