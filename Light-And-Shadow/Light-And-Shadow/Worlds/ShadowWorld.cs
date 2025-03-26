using Light_And_Shadow.Behaviors;
using Light_And_Shadow.Materials;
using OpenTK.Mathematics;

namespace Light_And_Shadow.Worlds;

public class ShadowWorld : World
{
    public ShadowWorld(Game game) : base(game) { }
    
    private GameObject staticCube;
    private GameObject rotatingCube;
    
    protected override void ConstructWorld()
    {
        base.ConstructWorld();

        staticCube = new GameObjectBuilder(Game)
            .Model("Room")
            .Material(new mat_default())
            .Position(0f, -2f, 0f)
            .Scale(5, 5, 5)
            .Build();

        rotatingCube = new GameObjectBuilder(Game)
            .Model("SmoothCube")
            .Material(new mat_default())
            .Position(1.5f, 0f, 0f)
            //.Behavior<RotateObjectBehavior>(Vector3.UnitY, 20f)
            //.Behavior<RotateObjectBehavior>(Vector3.UnitZ, 20f)// weird specular light
            .Build();

        GameObjects.Add(staticCube);
        GameObjects.Add(rotatingCube);
    }
}