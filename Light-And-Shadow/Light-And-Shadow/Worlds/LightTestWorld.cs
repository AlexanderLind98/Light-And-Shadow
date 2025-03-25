using Light_And_Shadow.Behaviors;
using Light_And_Shadow.Materials;
using OpenTK.Mathematics;

namespace Light_And_Shadow.Worlds;

public class LightTestWorld(Game game) : World(game)
{
    protected override void ConstructWorld()
    {
        base.ConstructWorld();

        GameObjects.Add(new GameObjectBuilder(Game)
            .Model("Room")
            .Material(new mat_concrete())
            .Position(0, -2, 1)
            .Scale(5, 5, 5)
            .Build());

        GameObjects.Add(new GameObjectBuilder(Game)
            .Model("Monkey")
            .Material(new mat_chrome())
            .Position(-2, 0, 0)
            .Behavior<RotateObjectBehavior>(Vector3.UnitY, 15f) // Y-axis, 90°/sec
            .Build());

        GameObjects.Add(new GameObjectBuilder(Game)
            .Model("SmoothCube")
            .Material(new mat_gold())
            .Position(2, 0, 0)
            .Behavior<RotateObjectBehavior>(Vector3.UnitX, -15f) // X-axis, -45°/sec
            .Build());

        GameObjects.Add(new GameObjectBuilder(Game)
            .Model("Statue")
            .Material(new mat_concrete())
            .Scale(4, 4, 4)
            .Position(0, -2, -10)
            .Build());
    }
}