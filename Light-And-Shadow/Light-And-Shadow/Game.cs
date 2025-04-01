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
        // shadow
        private int depthMapFBO;
        private int shadowMap;
        private Shader depthShader;
        private Matrix4 lightSpaceMatrix;
        //private Vector3 lightPos = new Vector3(0.0f, 2.0f, -3.0f);
        // private Vector3 lightPos = new Vector3(-8, 5, 0);
        // Vector3 lightDir = new Vector3(1f, -0.5f, 0f);
        private Vector3 lightPos;
        private Vector3 lightDir;
        

        private Texture shadowTexture;

        // shadow debug
        private Shader debugShader;
        private Mesh quadMesh;

        
        private int debugMode = 0;
 
        private List<GameObject> gameObjects = new List<GameObject>();
        private Camera camera;

        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            CenterWindow();
            GL.ClearColor(Color4.Gray);
        }
        
        protected override void OnLoad()
        {
            base.OnLoad();
            GL.Enable(EnableCap.DepthTest);
            lightPos = new Vector3(0.0f, 5.0f, 5.0f);      // just above camera spawn
            lightDir = new Vector3(0.0f, -0.5f, -1.0f).Normalized(); // down and forward


            SetupFrameBuffer();

            SetupDebugQuad();

            lightTest();

            SetupCamera();
        }

        private void SetupDebugQuad()
        {
            debugShader = new Shader("Shaders/shadowDebugQuad.vert", "Shaders/shadowDebugQuad.frag");
            quadMesh = new QuadMesh();
        }

        private void SetupFrameBuffer()
        {
            depthShader = new Shader("Shaders/shadowDepth.vert", "Shaders/shadowDepth.frag");
            
            depthMapFBO = GL.GenFramebuffer();

            // Opret depth texture
            shadowMap = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, shadowMap);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent,
                1024, 1024, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);

            // Konfigurer texture parametre
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, new float[] { 1, 1, 1, 1 });

            // Bind til framebuffer
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, depthMapFBO);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, shadowMap, 0);
            GL.DrawBuffer(DrawBufferMode.None);
            GL.ReadBuffer(ReadBufferMode.None);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            
            shadowTexture = new Texture(shadowMap);
        }

        private void lightTest()
        {
            Material sharedMaterial = new Material("Shaders/shadow.vert", "Shaders/shadow.frag");
            
            // var lightCube = new GameObject(this)
            // {
            //     Renderer = new Renderer(new Material("Shaders/OLD/ambientLightShader.vert", "Shaders/OLD/ambientLightShader.frag"), new CubeMesh()),
            //     Transform = 
            //     {
            //         Position = lightPos,
            //         Scale = new Vector3(0.2f)
            //     }
            // };
            // gameObjects.Add(lightCube);

          
            Renderer cubeRenderer = new Renderer(sharedMaterial, new CubeMesh());
            GameObject cubeObject = new GameObject(this)
            {
                Renderer = cubeRenderer,
                Transform =
                {
                    // position a bit below middle closer to camera
                    Position = new Vector3(0.0f, -2.0f, 1.0f),
                    Scale = new Vector3(1.0f)
                }
            };
            cubeObject.AddComponent<MoveObjectBehaviour>();
            gameObjects.Add(cubeObject);
            
            // Gulv
            Renderer floorRenderer = new Renderer(sharedMaterial, new CubeMesh());
            GameObject floor = new GameObject(this)
            {
                Renderer = floorRenderer,
                Transform =
                {
                    Position = new Vector3(0.0f, -4.0f, 0.0f),
                    Scale = new Vector3(10.0f, 0.5f, 10.0f)
                }
            };
            gameObjects.Add(floor);

            // Ekstra statiske kuber
            for (int i = 0; i < 3; i++)
            {
                Renderer r = new Renderer(sharedMaterial, new CubeMesh());
                GameObject staticCube = new GameObject(this)
                {
                    Renderer = r,
                    Transform =
                    {
                        Position = new Vector3(i * 2.5f - 2.5f, -3.0f, -3.0f),
                        Scale = new Vector3(1.0f)
                    }
                };
                gameObjects.Add(staticCube);
            }
        }

        /// <summary>
        /// Sets up the main camera.
        /// </summary>
        private void SetupCamera()
        {
            GameObject cameraObject = new GameObject(this);
            cameraObject.AddComponent<Camera>(60.0f, (float)Size.X, (float)Size.Y, 0.3f, 1000.0f);
            cameraObject.AddComponent<CamMoveBehavior>();
            camera = cameraObject.GetComponent<Camera>();
            gameObjects.Add(cameraObject);

            //Grab focus for cursor, locking it to window
            //CursorState = CursorState.Grabbed;
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            foreach (var obj in gameObjects)
            {
                obj.Update(args);
            }
            
            var input = KeyboardState;

            // Shader Debug mode switch
            if (input.IsKeyPressed(Keys.F1)) debugMode = 1;
            if (input.IsKeyPressed(Keys.F2)) debugMode = 2;
            if (input.IsKeyPressed(Keys.F3)) debugMode = 3;
            if (input.IsKeyPressed(Keys.F4)) debugMode = 0;


            if (input.IsKeyPressed(Keys.Escape))
            {
                Close();
            }
            
            // Update window title with debug mode
            Title = debugMode switch
            {
                1 => "Debug Mode: Ambient",
                2 => "Debug Mode: Diffuse",
                3 => "Debug Mode: Specular",
                _ => "Debug Mode: Combined"
            };
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            RenderShadows();

            RenderDebugQuad();

            Matrix4 viewProjection = camera.GetViewProjection();
            Vector3 cameraPos = camera.Position;
            
            foreach (var obj in gameObjects)
            {
                obj.Draw(viewProjection, cameraPos, debugMode, lightSpaceMatrix, shadowTexture);
            }
            
            SwapBuffers();
        }

        private void RenderDebugQuad()
        {
            // Gem eksisterende viewport
            int[] fullViewport = new int[4];
            GL.GetInteger(GetPName.Viewport, fullViewport);

            // Sæt viewport til 1/4 skærmstørrelse (nederste venstre hjørne)
            GL.Viewport(0, 0, Size.X / 4, Size.Y / 4);

            debugShader.Use();
            debugShader.SetInt("depthMap", 0);
            shadowTexture.Use(); // binder shadowMap til Texture0
            quadMesh.Draw();

            // Gendan viewport til fuld skærm
            GL.Viewport(fullViewport[0], fullViewport[1], fullViewport[2], fullViewport[3]);
        }


        private void RenderShadows()
        {
            // Gem den nuværende viewport
            int[] viewport = new int[4];
            GL.GetInteger(GetPName.Viewport, viewport);

            // Opret view-matrix for lyset, Lyset placeres i 'lightPos' og kigger i retning af 'lightDir'.
            Matrix4 lightView = Matrix4.LookAt(
                lightPos,
                lightPos + lightDir,
                Vector3.UnitY);

            // Opret en orthografisk projektionsmatrix til shadow mapping.
            Matrix4 lightProjection = Matrix4.CreateOrthographicOffCenter(
                -10.0f, 10.0f,   // venstre/højre
                -10.0f, 10.0f,   // bund/top
                1.0f, 50.0f);    // nær/fjern klippeafstande

            // Kombinér view og lysprojection til lys space, brugt til at beregne skygger.
            lightSpaceMatrix = lightView * lightProjection;

            GL.Viewport(0, 0, 1024, 1024);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, depthMapFBO);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            depthShader.Use();
            depthShader.SetMatrix("lightSpaceMatrix", lightSpaceMatrix);

            foreach (var obj in gameObjects)
            {
                if (obj.Renderer != null)
                {
                    var model = obj.Transform.CalculateModel();
                    depthShader.SetMatrix("model", model);
                    obj.Renderer.Mesh.Draw();
                }
            }

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            // Gendan viewport
            GL.Viewport(viewport[0], viewport[1], viewport[2], viewport[3]);
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