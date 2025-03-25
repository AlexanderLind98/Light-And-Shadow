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

            Material.SetUniform("dirLight.direction", currentWorld.SunDirection);
            Material.SetUniform("dirLight.ambient", currentWorld.GetSkyColor());
            Material.SetUniform("dirLight.diffuse", currentWorld.SunColor);
            Material.SetUniform("dirLight.specular", currentWorld.SunColor);

            // Set the transformed light direction
            Material.SetUniform("spotLight.position", camera.Position);
            Material.SetUniform("spotLight.direction", Vector3.Normalize(camera.Front));
            Material.SetUniform("spotLight.cutOff", (float)MathHelper.Cos(MathHelper.DegreesToRadians(12.5f))); //12,5° radius
            Material.SetUniform("spotLight.outerCutOff", (float)MathHelper.Cos(MathHelper.DegreesToRadians(30.5f))); //12,5° radius
            
            Material.SetUniform("spotLight.ambient", currentWorld.GetSkyColor());
            Material.SetUniform("spotLight.diffuse", new Vector3(1f, 1f, 1f));
            Material.SetUniform("spotLight.specular", new Vector3(1f, 1f, 1f));
            
            Material.SetUniform("spotLight.constant", 1.0f);
            Material.SetUniform("spotLight.linear", 0.09f);
            Material.SetUniform("spotLight.quadratic", 0.032f);

            for (int i = 0; i < 4; i++)
            {
                Material.SetUniform($"pointLights[{i}].ambient", currentWorld.GetSkyColor());
                Material.SetUniform($"pointLights[{i}].diffuse", new Vector3(0f, 1f, 1f));
                Material.SetUniform($"pointLights[{i}].specular", new Vector3(0f, 1f, 1f));
                
                Material.SetUniform($"pointLights[{i}].constant", 1.0f);
                Material.SetUniform($"pointLights[{i}].linear", 0.09f);
                Material.SetUniform($"pointLights[{i}].quadratic", 0.032f);
            }
            
            Material.SetUniform($"pointLights[0].position", new Vector3(0, 0, 0));
            Material.SetUniform($"pointLights[1].position", new Vector3(5, 5, 5));
            Material.SetUniform($"pointLights[2].position", new Vector3(-5, -5, -5));
            Material.SetUniform($"pointLights[3].position", new Vector3(0, 5, -10));
            
            Material.SetUniform("viewPos", (Vector3)camera.Position);

            Material.SetUniform("debugMode", currentDebugMode);
            
            Mesh.Draw();
        }
    }
}