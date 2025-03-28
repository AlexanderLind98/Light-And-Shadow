// === FILNAVN: ShadowMapDebugRenderer.cs ===

using Light_And_Shadow.Components;
using Light_And_Shadow.Materials;
using Light_And_Shadow.Shapes;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Light_And_Shadow;

public class ShadowMapDebugRenderer
{
    private Renderer _renderer;
    private GameObject _quad;
    private Material _material;

    public ShadowMapDebugRenderer(Game game, int depthTextureHandle)
    {
        // 1. Shader og materiale
        _material = new Material("Shaders/shadowMapQuad.vert", "Shaders/shadowMapQuad.frag");
        _material.SetUniform("shadowMap", 0); // Texture unit 0

        // 2. Mesh og Renderer
        var quadMesh = new QuadMesh();
        _renderer = new Renderer(_material, quadMesh);

        // 3. GameObject (uden world)
        _quad = new GameObject(game)
        {
            Renderer = _renderer
        };
 
        float quadSize = 0.75f;
        float half = quadSize / 2f;

        _quad.Transform.Scale = new Vector3(quadSize, quadSize, 1f);
        _quad.Transform.Position = new Vector3(-1f + half, -1f + half, 0f);

    }

    public void Draw(int depthMapTextureHandle)
    {
        // Bind shadowMap til texture unit 0
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, depthMapTextureHandle);

        Matrix4 model = _quad.Transform.CalculateModel();
        Matrix4 mvp = model;


        _material.UseShader();
        _material.SetUniform("mvp", mvp);
        _renderer.Mesh.Draw();
    }
}