using Light_And_Shadow.Behaviors;
using Light_And_Shadow.Components;
using Light_And_Shadow.Materials;
using Light_And_Shadow.Shapes;
using OpenTK_OBJ;
using OpenTK.Mathematics;

namespace Light_And_Shadow.Worlds;

public class LightTestWorld(Game game) : World(game)
{
    protected override void ConstructWorld()
    {
        base.ConstructWorld();

        (GameObject roomObject, Mesh roomMesh) = GameObjectFactory.CreateObjModel(Game, "Room");
        Renderer roomRenderer = new Renderer(new mat_concrete(), roomMesh);
        roomObject.Renderer = roomRenderer;
        GameObjects.Add(roomObject);
        
        (GameObject cube1Object, Mesh model1Mesh) = GameObjectFactory.CreateObjModel(Game, "Cube");
        (GameObject cube2Object, Mesh model2Mesh) = GameObjectFactory.CreateObjModel(Game, "Cube");

        roomObject.Transform.Position += new Vector3(0, -2, 1);
        roomObject.Transform.Scale = new Vector3(5, 5, 5);
        cube1Object.Transform.Position += new Vector3(-2, 0, 0);
        cube2Object.Transform.Position += new Vector3(2, 0, 0);
        // cube2Object.Transform.Rotation += new Vector3(1.0f, 0.0f, 0.0f);

        // RotateObjectBehavior rotator = new RotateObjectBehavior(cube2Object, Game);
        cube2Object.AddComponent<RotateObjectBehavior>();
        
        Renderer cube1Renderer = new Renderer(new mat_chrome(), model1Mesh);
        Renderer cube2Renderer = new Renderer(new mat_gold(), model2Mesh);
        
        cube1Object.Renderer = cube1Renderer;
        cube2Object.Renderer = cube2Renderer;

        GameObjects.Add(cube1Object);
        GameObjects.Add(cube2Object);
        
        //game.Title = "Light Test";
    }
}