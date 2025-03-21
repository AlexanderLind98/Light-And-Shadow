using Light_And_Shadow;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;


GameWindowSettings settings = new GameWindowSettings()
{
    UpdateFrequency = 60.0
};

NativeWindowSettings nativeWindowSettings = new NativeWindowSettings()
{
    ClientSize = new Vector2i(1920, 1080),
    Title = "OBJ Viewer"
};


using Game game = new Game(settings, nativeWindowSettings);
game.Run();