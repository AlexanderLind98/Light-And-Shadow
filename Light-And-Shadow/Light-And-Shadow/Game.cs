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
        private World currentWorld;

        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            CenterWindow();
            GL.ClearColor(Color4.CornflowerBlue);
            
            currentWorld = new ArchesWorld(this);
            // currentWorld = new LightTest(this);
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

            // Proof of concept - able to toggle between worlds
            if (input.IsKeyPressed(Keys.Enter))
            {
                currentWorld.UnloadWorld();
                if(Title == "Arches")
                {
                    currentWorld = new TestWorld(this);
                    currentWorld.LoadWorld();
                }
                else if (Title == "Test World")
                {
                    currentWorld = new ArchesWorld(this);
                    currentWorld.LoadWorld();
                }
            }

            if (input.IsKeyPressed(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            
            currentWorld.DrawWorld(args);
            
            SwapBuffers();
        }

        protected override void OnUnload()
        {
            currentWorld.UnloadWorld();
            
            base.OnUnload();
        }
    }
}