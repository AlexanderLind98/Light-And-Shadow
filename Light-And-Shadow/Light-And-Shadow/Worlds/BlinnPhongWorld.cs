using Light_And_Shadow.Behaviors;
using Light_And_Shadow.Materials;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Light_And_Shadow.Worlds;

public class BlinnPhongWorld : World
{
    private GameObject staticCube;
    private GameObject rotatingCube;

    public BlinnPhongWorld(Game game) : base(game)
    {
        WorldName = "Phong to Blinn-Phong";
    }
    
    public override string DebugLabel
    {
        get
        {
            return Game.DebugMode switch
            {
                5 => "Phong",
                6 => "Blinn-Phong",
                _ => "Phong"
            };
        }
    }

    protected override void ConstructWorld()
    {
        base.ConstructWorld();

        // GameObjects.Add(new GameObjectBuilder(Game)
        //     .Model("Monkey")
        //     .Material(new mat_gold())
        //     .Position(0, 0, 0)
        //     .Behavior<RotateObjectBehavior>(Vector3.UnitY, 10f)
        //     .Build());
        
        staticCube = new GameObjectBuilder(Game)
            .Model("Cube")
            .Material(new mat_gold_simple())
            .Position(-1.5f, 0f, 0f)
            .Build();

        rotatingCube = new GameObjectBuilder(Game)
            .Model("SmoothCube")
            .Material(new mat_gold_simple())
            .Position(1.5f, 0f, 0f)
            .Behavior<RotateObjectBehavior>(Vector3.UnitX, 20f)
            //.Behavior<RotateObjectBehavior>(Vector3.UnitY, 20f)
            //.Behavior<RotateObjectBehavior>(Vector3.UnitZ, 20f)// weird specular light
            .Build();

        GameObjects.Add(staticCube);
        GameObjects.Add(rotatingCube);
    }

    public override void HandleInput(KeyboardState input)
    {
        base.HandleInput(input);
        
        if (input.IsKeyPressed(Keys.D5))
        {
            staticCube.Renderer.Material = new mat_gold_simple();
            staticCube.Renderer.Material.UpdateUniforms();

            rotatingCube.Renderer.Material = new mat_gold_simple();
            rotatingCube.Renderer.Material.UpdateUniforms();

            Game.DebugMode = 5; // phong
        }
        
        if (input.IsKeyPressed(Keys.D6))
        {
            staticCube.Renderer.Material = new mat_gold();
            staticCube.Renderer.Material.UpdateUniforms();

            rotatingCube.Renderer.Material = new mat_gold();
            rotatingCube.Renderer.Material.UpdateUniforms();

            Game.DebugMode = 6; // Blinn-Phong
        }
    }
}