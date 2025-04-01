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

        public void Draw(Matrix4 model, Matrix4 viewProjection, Vector3 cameraPosition, int currentDebugMode, Matrix4 lightSpaceMatrix, Texture shadowMap)
        {
            Material.UseShader();
            Material.SetUniform("model", model);
            Material.SetUniform("viewProjection", viewProjection);
            Material.SetUniform("lightPos", new Vector3(0.0f, 2.0f, -3.0f));
            Material.SetUniform("lightColor", Vector3.One);
            Material.SetUniform("ambientStrength", 0.2f);
            Material.SetUniform("objectColor", new Vector3(1.0f, 0.5f, 0.3f));
            Material.SetUniform("viewPos", cameraPosition);
            Material.SetUniform("shininess", 256.0f);
            Material.SetUniform("specularStrength", 10.0f);
            Material.SetUniform("debugMode", currentDebugMode); 

            // shadow
            Material.SetUniform("lightSpaceMatrix", lightSpaceMatrix);
            Material.SetUniform("shadowMap", shadowMap); 
            
            Mesh.Draw();
        }
    }
}