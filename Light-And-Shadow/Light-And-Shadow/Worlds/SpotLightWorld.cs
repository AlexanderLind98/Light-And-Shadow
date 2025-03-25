using Light_And_Shadow.Materials;

namespace Light_And_Shadow.Worlds;

public class SpotLightWorld : World
{
    public SpotLightWorld(Game game) : base(game) { }

    public override string DebugLabel
    {
        get
        {
            return "Spotlight On";
        }
    }

    protected override void ConstructWorld()
    {
        base.ConstructWorld();

        GameObjects.Add(new GameObjectBuilder(Game)
            .Model("Statue")
            .Material(new mat_marble())
            .Scale(3, 3, 3)
            .Position(0, -2, -5)
            .Build());

    }
}