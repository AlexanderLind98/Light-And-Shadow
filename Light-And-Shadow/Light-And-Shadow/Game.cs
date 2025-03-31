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

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            KeyboardState input = KeyboardState;
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

            if (input.IsKeyPressed(Keys.Escape))
            {
                Close();
            }

            Title = $"{currentWorld.WorldName} | {currentWorld.DebugLabel}";
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

            
            
            // Render to depth map
            GL.Viewport(0, 0, shadowFramebuffer.SHADOW_WIDTH, shadowFramebuffer.SHADOW_HEIGHT);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, shadowFramebuffer.depthMapFBO);
            GL.Clear(ClearBufferMask.DepthBufferBit);
            Vector3 monkeyFrontLightDir = new Vector3(0f, 0f, -1f); // Lys peger mod monkey langs -Z
            shadowFramebuffer.ConfigureShaderFromDirection(currentWorld.DirectionalLight.Transform.Rotation);
        
            //Render depth map from scene
            currentWorld.RenderShadowMap(shadowFramebuffer.lightSpaceMatrix, shadowFramebuffer.simpleDepthShader);
        
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        
            // Render scene as normal using the shadow map
            GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            shadowFramebuffer.ConfigureShaderFromDirection(currentWorld.DirectionalLight.Transform.Rotation);
            
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, shadowFramebuffer.depthMap);
            currentWorld.ShadowMatrix = shadowFramebuffer.lightSpaceMatrix;

          
            
            //Render scene as normal
            currentWorld.DrawWorld(args, DebugMode);
            debugRenderer.Draw(shadowFramebuffer.depthMap);

            SwapBuffers();
        }

        protected override void OnUnload()
        {
            currentWorld.UnloadWorld();
            shadowFramebuffer.Dispose();
            base.OnUnload();
        }
    }
}