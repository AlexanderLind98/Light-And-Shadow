using Light_And_Shadow.Behaviors;
using Light_And_Shadow.Components;
using Light_And_Shadow.Shapes;
using OpenTK_OBJ;
using OpenTK.Mathematics;

namespace Light_And_Shadow.Worlds;

public class LightTest(Game game) : World(game)
{
    protected override void ConstructWorld()
    {
        base.ConstructWorld();
        
        // Material cubeMaterial = new Material("Shaders/lightShader.vert", "Shaders/lightShader.frag");
        Material cubeMaterial = new Material("Shaders/shader.vert", "Shaders/shader.frag");
        Renderer cubeRenderer = new Renderer(cubeMaterial, new CubeMesh());
        GameObject cubeObject = new GameObject(Game)
        {
            Renderer = cubeRenderer,
            Transform =
            {
                Position = new Vector3(1, 1, 1)
            }
        };
        cubeObject.AddComponent<MoveObjectBehaviour>();
        GameObjects.Add(cubeObject);
        
        game.Title = "Light Test";
    }
}