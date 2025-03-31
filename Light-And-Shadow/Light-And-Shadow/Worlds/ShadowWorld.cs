using Light_And_Shadow.Behaviors;
using Light_And_Shadow.Materials;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Light_And_Shadow.Worlds;

public class ShadowWorld : World
{
    public ShadowWorld(Game game) : base(game) {  }
    
    private GameObject staticCube;
    private GameObject rotatingCube;
    
    protected override void ConstructWorld()
    {
        base.ConstructWorld();

        // staticCube = new GameObjectBuilder(Game)
        //     .Model("GroundFloor")
        //     .Material(new mat_gold())
        //     .Position(0f, -1f, 0f)
        //     .Scale(5, 1, 5)
        //     .Build();
        //
        // // GameObjects.Add(GameObjectFactory.CreateCube(Game));
        //
        // rotatingCube = new GameObjectBuilder(Game)
        //     .Model("SmoothCube")
        //     .Material(new mat_marble())
        //     .Position(1.5f, 0f, 0f)
        //     .Behavior<RotateObjectBehavior>(Vector3.UnitX, 20f)
        //     .Build();
        //
        // GameObjects.Add(staticCube);
        // GameObjects.Add(rotatingCube);
        
        DirectionalLight.LightColor = Vector3.Zero;
        
        var monkey = new GameObjectBuilder(Game)
            .Model("Monkey")
            .Material(new mat_gold()) 
            .Position(0f, 0f, 0f)
            .Behavior<RotateObjectBehavior>(Vector3.UnitY, 10f)
            .Build();

        GameObjects.Add(monkey);
    }
    
    public override void HandleInput(KeyboardState input)
    {
        const float step = 0.5f;

        if (input.IsKeyDown(Keys.Left))  SunDirection.X -= step;
        if (input.IsKeyDown(Keys.Right)) SunDirection.X += step;
        if (input.IsKeyDown(Keys.Up))    SunDirection.Y += step;
        if (input.IsKeyDown(Keys.Down))  SunDirection.Y -= step;
        if (input.IsKeyDown(Keys.PageUp)) SunDirection.Z += step;
        if (input.IsKeyDown(Keys.PageDown)) SunDirection.Z -= step;

        DirectionalLight.Transform.Rotation = SunDirection;
        DirectionalLight.UpdateVisualizer(this);
    }

}