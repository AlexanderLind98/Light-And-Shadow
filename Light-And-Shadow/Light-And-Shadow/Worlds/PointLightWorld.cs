using Light_And_Shadow.Behaviors;
using Light_And_Shadow.Lights;
using Light_And_Shadow.Materials;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Light_And_Shadow.Worlds;

public class PointLightWorld : World
{
    private GameObject staticCube;
    private GameObject rotatingCube;

    public PointLightWorld(Game game) : base(game)
    {
        WorldName = "Point Light World";
        SkyColor = Color4.Black;
    }

    protected override void ConstructWorld()
    {
        base.ConstructWorld();

        // GameObjects.Add(new GameObjectBuilder(Game)
        //     .Model("Monkey")
        //     .Material(new mat_gold())
        //     .Position(0, 0, 0)
        //     .Behavior<RotateObjectBehavior>(Vector3.UnitY, 10f)
        //     .Build());
        
        /*GameObjects.Add(new GameObjectBuilder(Game)
            .Model("Room")
            .Material(new mat_concrete())
            .Position(0, -2, 1)
            .Scale(5, 5, 5)
            .Build());*/
        
        staticCube = new GameObjectBuilder(Game)
            .Model("Cube")
            .Material(new mat_gold())
            .Position(-1.5f, 0f, 0f)
            .Build();

        rotatingCube = new GameObjectBuilder(Game)
            .Model("SmoothCube")
            .Material(new mat_gold())
            .Position(1.5f, 0f, 0f)
            .Behavior<RotateObjectBehavior>(Vector3.UnitX, 20f)
            //.Behavior<RotateObjectBehavior>(Vector3.UnitY, 20f)
            //.Behavior<RotateObjectBehavior>(Vector3.UnitZ, 20f)// weird specular light
            .Build();
        PointLight pointLight = new PointLight(this, Color4.Red, 2.0f);
        pointLight.Transform.Position = new Vector3(1f, 0f, 2f);
        pointLight.UpdateVisualizer(this);
        PointLights.Add(pointLight);

        GameObjects.Add(staticCube);
        GameObjects.Add(rotatingCube);
    }

    public override void HandleInput(KeyboardState input)
    {
        base.HandleInput(input);
        
        /*if (input.IsKeyPressed(Keys.D5))
        {
            staticCube.Renderer.Material = new mat_gold_simple();
            staticCube.Renderer.Material.UpdateUniforms();

            rotatingCube.Renderer.Material = new mat_gold_simple();
            rotatingCube.Renderer.Material.UpdateUniforms();
        }
        
        if (input.IsKeyPressed(Keys.D6))
        {
            staticCube.Renderer.Material = new mat_gold();
            staticCube.Renderer.Material.UpdateUniforms();

            rotatingCube.Renderer.Material = new mat_gold();
            rotatingCube.Renderer.Material.UpdateUniforms();
        }*/
    }
}