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
    
    public Matrix4 ShadowMatrix { get; set; }


    protected World(Game game)
    {
        Game = game;
        SetupCamera();
        
        //Basic Sun
        DirectionalLight = new DirectionalLight(this, Color4.LightYellow, 1);
        DirectionalLight.Transform.Rotation = SunDirection;
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
        ConstructWorld();
        GL.ClearColor(SkyColor);
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

        Matrix4 viewProjection = camera.GetViewProjection();
        
        Vector3 cameraPos = camera.Position;
        foreach (var obj in GameObjects)
        {
            obj.Draw(viewProjection, camera, this, debugMode);
        }
    }

    public void RenderShadowMap(Matrix4 lightSpaceMatrix, Shader depthShader)
    {
        foreach (var obj in GameObjects)
        {
            obj.DrawDepth(depthShader, lightSpaceMatrix);
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