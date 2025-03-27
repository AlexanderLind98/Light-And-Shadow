namespace Light_And_Shadow.Worlds;

public class SimpleShapesWorld : World
{
    public SimpleShapesWorld(Game game) : base(game)
    {
        WorldName = "simple shapes";
    }

    protected override void ConstructWorld()
    {
        base.ConstructWorld();
        GameObjects.Add(GameObjectFactory.CreateTriangle(Game));
    }
}