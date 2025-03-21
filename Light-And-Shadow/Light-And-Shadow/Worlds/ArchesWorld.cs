using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Light_And_Shadow.Worlds;

public class ArchesWorld(Game game) : World(game)
{
    protected override void ConstructWorld()
    {
        base.ConstructWorld();
        GameObjects.Add(GameObjectFactory.CreateObjModel(Game, "Arches"));
        
        GL.ClearColor(Color4.DarkGoldenrod);
        
        game.Title = "Arches";
    }
}