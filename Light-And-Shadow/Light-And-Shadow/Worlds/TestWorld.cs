using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Light_And_Shadow.Worlds;

public class TestWorld(Game game) : World(game)
{
    protected override void ConstructWorld()
    {
        base.ConstructWorld();
        GameObject cube = GameObjectFactory.CreateObjModel(Game, "Cube");
        cube.Transform.Position += new Vector3(0, 5, 0);
        GameObjects.Add(cube);
        
        GL.ClearColor(Color4.CornflowerBlue);
        
        game.Title = "Test World";
    }
}