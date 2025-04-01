using Light_And_Shadow.Behaviors;
using Light_And_Shadow.Lights;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Light_And_Shadow.Worlds;

public abstract class World
{
    public readonly List<GameObject> GameObjects = [];
    public readonly Game Game;

    public virtual string DebugLabel => "Combined";
    public string WorldName { get; set; }

    private Camera camera;

    public Vector3 SunDirection = new Vector3(-0.2f, -1.0f, -0.3f); //Set default sun direction;
    public Vector3 SunColor = new Vector3(2f, 2f, 1.8f); //Set default sun direction;
    public Color4 SkyColor = Color4.CornflowerBlue;
    
    public DirectionalLight DirectionalLight;
    public List<PointLight> PointLights = new();
    public List<SpotLight> SpotLights = new();

    private Shader dir_depthShader;
    private int dir_depthMapFBO;
    public DepthTexture depthMap;

    protected World(Game game)
    {
        Game = game;
        SetupCamera();
        
        //Basic Sun
        DirectionalLight = new DirectionalLight(this, Color4.LightYellow, 1);
        DirectionalLight.Transform.Rotation = SunDirection;
        DirectionalLight.Transform.Position = new Vector3(2, 5, -2);
        DirectionalLight.UpdateVisualizer(this);
    }

    /// <summary>
    /// Method used for constructing initial world
    /// </summary>
    protected virtual void ConstructWorld() { }
    
    public virtual void HandleInput(KeyboardState input) { }

    public Vector3 GetSkyColor()
    {
        return new Vector3(SkyColor.R, SkyColor.G, SkyColor.B);
    }
    
    public Vector3 GetSkyColor(float intensity)
    {
        return new Vector3(SkyColor.R + intensity, SkyColor.G + intensity, SkyColor.B + intensity);
    }

    public void LoadWorld()
    {
        GL.Enable(EnableCap.DepthTest);
        GL.ClearColor(SkyColor);

        //Depth shader
        dir_depthShader = new Shader("Shaders/DirDepth.vert", "Shaders/DirDepth.frag");
        dir_depthMapFBO = GL.GenTexture();
        depthMap = new DepthTexture(dir_depthMapFBO, 1024, 1024);
        
        GL.Enable(EnableCap.DepthTest);
        GL.Enable(EnableCap.CullFace);
        
        ConstructWorld();
    }

    public void UpdateWorld(FrameEventArgs args)
    {
        foreach (var obj in GameObjects)
        {
            obj.Update(args);
        }
    }

    public void DrawWorld(FrameEventArgs args, int debugMode)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        GL.CullFace(TriangleFace.Front);
        
        Matrix4 lightProjection = Matrix4.CreateOrthographicOffCenter(-10.0f, 10.0f, -10, 10, 0.1f, 10.0f);
        Matrix4 lightView = Matrix4.LookAt(new Vector3(DirectionalLight.Transform.Position),
            new Vector3(DirectionalLight.Transform.Position + DirectionalLight.Transform.Rotation),
            new Vector3(0.0f, 1.0f, 0.0f));
        Matrix4 lightSpaceMatrix = lightView * lightProjection;
        
        depthMap.Use(TextureUnit.Texture0);
        dir_depthShader.Use();
        dir_depthShader.SetMatrix("lightSpaceMatrix", lightSpaceMatrix);
        
        GL.Viewport(0, 0, 1024, 1024);
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, dir_depthMapFBO);
        GL.Clear(ClearBufferMask.DepthBufferBit);
        
        GameObjects.ForEach(x => x.RenderDepth(dir_depthShader));
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        
        // reset viewport
        GL.CullFace(TriangleFace.Back);
        GL.Viewport(0, 0, Game.Size.X, Game.Size.Y);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        Matrix4 viewProjection = camera.GetViewProjection();
        
        Vector3 cameraPos = camera.Position;
        foreach (var obj in GameObjects)
        {
            obj.Draw(viewProjection, camera, this, debugMode);
        }
    }

    public void UnloadWorld()
    {
        foreach (var obj in GameObjects)
        {
            if (obj.Renderer != null)
            {
                obj.Renderer.Mesh?.Dispose();

                if (obj.Renderer.Material is IDisposable disposableMat)
                {
                    disposableMat.Dispose();
                }
            }
        }

        foreach (var obj in GameObjects)
        {
            obj.Dispose();
        }
        
        dir_depthShader.Dispose();

        GameObjects.Clear();
    }
    
    /// <summary>
    /// Sets up the main camera.
    /// </summary>
    private void SetupCamera()
    {
        GameObject cameraObject = new GameObject(Game);
        cameraObject.AddComponent<Camera>(60.0f, (float)Game.Size.X, (float)Game.Size.Y, 0.3f, 1000.0f);
        cameraObject.AddComponent<CamMoveBehavior>();
        camera = cameraObject.GetComponent<Camera>();
        GameObjects.Add(cameraObject);

        //Grab focus for cursor, locking it to window
        Game.CursorState = CursorState.Grabbed;
    }
}