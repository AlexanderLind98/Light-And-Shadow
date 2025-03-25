using Light_And_Shadow.Behaviors;
using Light_And_Shadow.Components;
using Light_And_Shadow.Shapes;
using Light_And_Shadow.Worlds;
using OpenTK_OBJ;
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
        private int debugMode = 0;
        
        private World currentWorld;

        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            CenterWindow();
            GL.ClearColor(Color4.CornflowerBlue);
            
            //currentWorld = new TestWorld(this);
            currentWorld = new MultiLightTestWorld(this);
        }
        
        protected override void OnLoad()
        {
            base.OnLoad();
            
            currentWorld.LoadWorld();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            currentWorld.UpdateWorld(args);
            
            KeyboardState input = KeyboardState;

            // // Proof of concept - able to toggle between worlds
            // if (input.IsKeyPressed(Keys.Enter))
            // {
            //     currentWorld.UnloadWorld();
            //     if(Title == "Arches")
            //     {
            //         currentWorld = new TestWorld(this);
            //         currentWorld.LoadWorld();
            //     }
            //     else if (Title == "Test World")
            //     {
            //         currentWorld = new ArchesWorld(this);
            //         currentWorld.LoadWorld();
            //     }
            // }

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
            
            currentWorld.DrawWorld(args, debugMode);
            
            SwapBuffers();
        }

        protected override void OnUnload()
        {
            currentWorld.UnloadWorld();
            
            base.OnUnload();
        }
    }
}