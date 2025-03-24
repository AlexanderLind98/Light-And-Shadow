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

        (GameObject cubeObject, Mesh modelMesh) = GameObjectFactory.CreateObjModel(Game, "Cube");

        cubeObject.Transform.Position += new Vector3(0, -2, 1);
        
        Material cubeMaterial = new mat_gold();
        Renderer cubeRenderer = new Renderer(cubeMaterial, modelMesh);
      
        cubeObject.Renderer = cubeRenderer;
        cubeObject.AddComponent<MoveObjectBehaviour>();
        GameObjects.Add(cubeObject);
        
        //game.Title = "Light Test";
    }
}