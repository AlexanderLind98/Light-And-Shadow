using Light_And_Shadow.Lights;
using Light_And_Shadow.Worlds;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

// For TriangleMesh and CubeMesh

namespace Light_And_Shadow
{
    public class Game : GameWindow
    {
        public int DebugMode { get; set; } = 0;

        private World currentWorld;
        private ShadowFramebuffer shadowFramebuffer;
        private ShadowMapDebugRenderer debugRenderer;


        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            CenterWindow();
            GL.ClearColor(Color4.Black);
            
            //currentWorld = new TestWorld(this);
            currentWorld = new ShadowWorld(this);
        }
        
        protected override void OnLoad()
        {
            base.OnLoad();
            shadowFramebuffer = new ShadowFramebuffer();
            debugRenderer = new ShadowMapDebugRenderer(this, shadowFramebuffer.depthMap);

            currentWorld.LoadWorld();
        }

        
        
        private void SwitchWorld(int index)
        {
            currentWorld.UnloadWorld();

            currentWorld = index switch
            {
                1 => new SimpleShapesWorld(this),
                2 => new LightTestWorld(this),
                3 => new PointLightWorld(this),
                4 => new SpotLightWorld(this),
                5 => new MultiLightTestWorld(this),
                //6 => new PresenterWorld(this), //TODO: Implement presenter models
                _ => new SimpleShapesWorld(this)
            };

            currentWorld.LoadWorld();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            var lightDir = Light.ConvertEulerToDirection(currentWorld.DirectionalLight.Transform.Rotation);
            shadowFramebuffer.ConfigureShaderFromDirection(lightDir);


            
            // var cam = currentWorld.ActiveCamera;
            // var camPosition = cam.Position;
            // var camDirection = cam.Front.Normalized();
            //
            // shadowFramebuffer.ConfigureShaderFromPositionAndDirection(camPosition, -camDirection);
            // Console.WriteLine("LightPos = " + camPosition + ", LightDir = " + -camDirection);

            
            // Render to depth map
            GL.Viewport(0, 0, shadowFramebuffer.SHADOW_WIDTH, shadowFramebuffer.SHADOW_HEIGHT);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, shadowFramebuffer.depthMapFBO);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            shadowFramebuffer.simpleDepthShader.Use();

            foreach (var go in currentWorld.GameObjects)
            {
                go.DrawDepth(shadowFramebuffer.simpleDepthShader, shadowFramebuffer.lightSpaceMatrix);
            }

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        
            // Render scene as normal using the shadow map
            GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, shadowFramebuffer.depthMap);
            
            //Render scene as normal
            currentWorld.DrawWorld(args, DebugMode);
            debugRenderer.Draw(shadowFramebuffer.depthMap);

            SwapBuffers();
        }
        
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            KeyboardState input = KeyboardState;
            MouseState mouse = MouseState;
            
            currentWorld.HandleInput(input); 

            currentWorld.UpdateWorld(args);

            if (input.IsKeyPressed(Keys.F1)) SwitchWorld(1);
            if (input.IsKeyPressed(Keys.F2)) SwitchWorld(2);
            if (input.IsKeyPressed(Keys.F3)) SwitchWorld(3);
            if (input.IsKeyPressed(Keys.F4)) SwitchWorld(4);
            if (input.IsKeyPressed(Keys.F5)) SwitchWorld(5);
            if (input.IsKeyPressed(Keys.F6)) SwitchWorld(6);


            // Shader Debug mode switch
            if (input.IsKeyPressed(Keys.D1)) DebugMode = 1;
            if (input.IsKeyPressed(Keys.D2)) DebugMode = 2;
            if (input.IsKeyPressed(Keys.D3)) DebugMode = 3;
            if (input.IsKeyPressed(Keys.D4)) DebugMode = 0;
            
            Vector2i mousePos = new Vector2i((int)mouse.X, (int)mouse.Y);
            if (mouse.IsButtonPressed(MouseButton.Left) && ClientRectangle.ContainsExclusive(mousePos) && CursorState == CursorState.Normal)
            {
                CursorState = CursorState.Grabbed;
            }

            if (input.IsKeyPressed(Keys.Escape))
            {
                Close();
            }

            Title = $"{currentWorld.WorldName} | {currentWorld.DebugLabel}";
        }
        

        protected override void OnUnload()
        {
            currentWorld.UnloadWorld();
            shadowFramebuffer.Dispose();
            base.OnUnload();
        }
    }
}