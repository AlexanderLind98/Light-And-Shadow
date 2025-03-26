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
            .Model("GroundFloor")
            .Material(new mat_gold())
            .Position(0f, -1f, 0f)
            .Scale(5, 1, 5)
            .Build();
        
        // GameObjects.Add(GameObjectFactory.CreateCube(Game));

        rotatingCube = new GameObjectBuilder(Game)
            .Model("SmoothCube")
            .Material(new mat_marble())
            .Position(1.5f, 0f, 0f)
            .Build();

        GameObjects.Add(staticCube);
        GameObjects.Add(rotatingCube);
    }
}