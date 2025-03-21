using Light_And_Shadow.Behaviors;
using Light_And_Shadow.Components;
using Light_And_Shadow.Shapes;
using OpenTK_OBJ;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;


namespace Light_And_Shadow
{
    public class Game : GameWindow
    {
        private List<GameObject> gameObjects = new List<GameObject>();
        private Camera camera;

        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            CenterWindow();
            GL.ClearColor(Color4.CornflowerBlue);
        }
        
        protected override void OnLoad()
        {
            base.OnLoad();
            GL.Enable(EnableCap.DepthTest);

            // Load game objects from factory.
            gameObjects.Add(GameObjectFactory.CreateTriangle(this));
            // gameObjects.Add(GameObjectFactory.CreateCube(this));
            //gameObjects.Add(GameObjectFactory.CreateObjModel(this));

            lightTest();

            SetupCamera();
        }

        private void lightTest()
        {
            Material cubeMaterial = new Material("Shaders/lightShader.vert", "Shaders/lightShader.frag");
            //Material cubeMaterial = new Material("Shaders/shader.vert", "Shaders/shader.frag");
            Renderer cubeRenderer = new Renderer(cubeMaterial, new CubeMesh());
            GameObject cubeObject = new GameObject(this)
            {
                Renderer = cubeRenderer,
                Transform =
                {
                    Position = new Vector3(1, 1, 1)
                }
            };
            cubeObject.AddComponent<MoveObjectBehaviour>();
            gameObjects.Add(cubeObject);
        }

        /// <summary>
        /// Sets up the main camera.
        /// </summary>
        private void SetupCamera()
        {
            GameObject cameraObject = new GameObject(this);
            cameraObject.AddComponent<Camera>(60.0f, (float)Size.X, (float)Size.Y, 0.3f, 1000.0f);
            camera = cameraObject.GetComponent<Camera>();
            gameObjects.Add(cameraObject);
        }


        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            foreach (var obj in gameObjects)
            {
                obj.Update(args);
            }
            
            KeyboardState input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 viewProjection = camera.GetViewProjection();
            foreach (var obj in gameObjects)
            {
                obj.Draw(viewProjection);
            }
            
            SwapBuffers();
        }

        protected override void OnUnload()
        {
            foreach (var obj in gameObjects)
            {
                if (obj.Renderer?.Mesh is IDisposable disposableMesh)
                {
                    disposableMesh.Dispose();
                }
            }
            
            base.OnUnload();
        }
    }
}