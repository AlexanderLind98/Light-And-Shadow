using System.Drawing;
using Light_And_Shadow.Behaviors;
using Light_And_Shadow.Lights;
using Light_And_Shadow.Materials;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Light_And_Shadow.Worlds;

public class LightTestWorld : World
{
    private GameObject room;
    private GameObject statue;
    private GameObject staticCube;
    private GameObject rotatingCube;

    public LightTestWorld(Game game) : base(game)
    {
        WorldName = "Light Test World";

        SkyColor = Color4.CornflowerBlue;
        // SunColor = Vector3.Zero;
        // DirectionalLight.LightColor = SunColor;
    }

    public override string DebugLabel
    {
        get
        {
            return Game.DebugMode switch
            {
                1 => "Ambient",
                2 => "Diffuse",
                3 => "Specular",
                _ => "Combined"
            };
        }
    }
    
    protected override void ConstructWorld()
    {
        base.ConstructWorld();
        
        room = new GameObjectBuilder(Game)
            .Model("Ground")
            .Material(new mat_marble())
            .Position(0f, -2f, 0f)
            .Scale(2, 2 ,2)
            .Build();
        
        statue = new GameObjectBuilder(Game)
            .Model("Statue")
            .Material(new mat_marble())
            .Position(0, -2f, -10f)
            .Scale(5f, 5f, 5f)
            .Build();

        staticCube = new GameObjectBuilder(Game)
            .Model("SmoothCube")
            .Material(new mat_box())
            .Position(-1.5f, 0f, 0f)
            .Build();

        rotatingCube = new GameObjectBuilder(Game)
            .Model("SmoothCube")
            .Material(new mat_chrome())
            .Position(1.5f, 0f, 0f)
            .Behavior<RotateObjectBehavior>(Vector3.UnitX, 1f)
            .Build();

        GameObjects.Add(room);
        GameObjects.Add(statue);
        GameObjects.Add(staticCube);
        GameObjects.Add(rotatingCube);
        
        new SpotLight(this, Color4.White, 1f, 15.0f, 20.0f);
        SpotLights[0].ToggleLight();
    }

    public override void HandleInput(KeyboardState input)
    {
        if (input.IsKeyPressed(Keys.D1))
        {
            Game.DebugMode = 1; // Ambient
        }

        if (input.IsKeyPressed(Keys.D2))
        {
            Game.DebugMode = 2; // Diffuse
        }

        if (input.IsKeyPressed(Keys.D3))
        {
            Game.DebugMode = 3; // Specular
        }

        if (input.IsKeyPressed(Keys.D4))
        {
            /*staticCube.Renderer.Material = new mat_gold_simple();
            staticCube.Renderer.Material.UpdateUniforms();

            rotatingCube.Renderer.Material = new mat_gold_simple();
            rotatingCube.Renderer.Material.UpdateUniforms();*/

            Game.DebugMode = 0; // Full lighting
        }
    }
}