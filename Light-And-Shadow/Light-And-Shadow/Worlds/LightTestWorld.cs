using Light_And_Shadow.Behaviors;
using Light_And_Shadow.Materials;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Light_And_Shadow.Worlds;

public class LightTestWorld : World
{
    private GameObject staticCube;
    private GameObject rotatingCube;

    public LightTestWorld(Game game) : base(game)
    {
        WorldName = "Light Test World";
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

        staticCube = new GameObjectBuilder(Game)
            .Model("Cube")
            .Material(new mat_default())
            .Position(-1.5f, 0f, 0f)
            .Build();

        rotatingCube = new GameObjectBuilder(Game)
            .Model("SmoothCube")
            .Material(new mat_default())
            .Position(1.5f, 0f, 0f)
            .Behavior<RotateObjectBehavior>(Vector3.UnitY, 20f)
            //.Behavior<RotateObjectBehavior>(Vector3.UnitZ, 20f)// weird specular light
            .Behavior<RotateObjectBehavior>(Vector3.UnitX, 20f)
            .Build();

        GameObjects.Add(staticCube);
        GameObjects.Add(rotatingCube);
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
            staticCube.Renderer.Material = new mat_gold_simple();
            staticCube.Renderer.Material.UpdateUniforms();

            rotatingCube.Renderer.Material = new mat_gold_simple();
            rotatingCube.Renderer.Material.UpdateUniforms();

            Game.DebugMode = 0; // Full lighting
        }
    }
}