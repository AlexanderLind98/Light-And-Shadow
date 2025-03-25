using Light_And_Shadow.Behaviors;
using Light_And_Shadow.Materials;
using OpenTK.Mathematics;

namespace Light_And_Shadow.Worlds;

public class PointLightWorld : World
{
    public PointLightWorld(Game game) : base(game) { }

    protected override void ConstructWorld()
    {
        base.ConstructWorld();

        GameObjects.Add(new GameObjectBuilder(Game)
            .Model("Monkey")
            .Material(new mat_gold())
            .Position(0, 0, 0)
            .Behavior<RotateObjectBehavior>(Vector3.UnitY, 10f)
            .Build());

        // Her ville du sætte shaderens uniforms til at benytte et pointlight (position i verden osv.)
    }
}