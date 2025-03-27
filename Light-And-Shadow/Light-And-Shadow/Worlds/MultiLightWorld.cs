using Light_And_Shadow.Behaviors;
using Light_And_Shadow.Lights;
using Light_And_Shadow.Materials;
using OpenTK_OBJ;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Light_And_Shadow.Worlds;

public class MultiLightWorld : World
{
    private int currentStage = 0;
    
    public MultiLightWorld(Game game) : base(game)
    {
        // SunColor = Vector3.Zero;
        // SkyColor = Color4.Black;
        
        DirectionalLight.ToggleLight();
    }
    
    protected override void ConstructWorld()
    {
        base.ConstructWorld();

        GameObjects.Add(new GameObjectBuilder(Game)
            .Model("Room")
            .Material(new mat_concrete())
            .Position(0, -5, 0)
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
            .Material(new mat_marble())
            .Scale(4, 4, 4)
            .Position(0, -5, -9)
            .Build());

        
        new PointLight(this, Color4.Red);
        PointLights[0].Transform.Position = new Vector3(-2, -1, 0);
        PointLights[0].UpdateVisualizer(this);
        PointLights[0].ToggleLight();
        
        new PointLight(this, Color4.Purple);
        PointLights[1].Transform.Position = new Vector3(3, 0, 2);
        PointLights[1].UpdateVisualizer(this);
        PointLights[1].ToggleLight();

        Transform spotlightTransform = new Transform();
        spotlightTransform.Position =  new Vector3(-3f, 8, -9);
        spotlightTransform.Rotation = new Vector3(0.25f, -1f, 0f);
        
        Transform spotlight1Transform = new Transform();
        spotlight1Transform.Position =  new Vector3(3f, -8, -9);
        spotlight1Transform.Rotation = new Vector3(-0.25f, 1f, 0f);
        
        //Flashlight disabled
        new SpotLight(this, Color4.White);
        SpotLights[0].ToggleLight();
        
        new SpotLight(this, Color4.White, spotlightTransform, 0.7f, 45f, 50f);
        SpotLights[1].ToggleLight();
        
        new SpotLight(this, Color4.Blue, spotlight1Transform, 0.7f, 45f, 50f);
        SpotLights[2].ToggleLight();
    }
    
    public override void HandleInput(KeyboardState input)
    {
        base.HandleInput(input);
        
        if (input.IsKeyPressed(Keys.V))
        {
            currentStage++;

            switch (currentStage)
            {
                case 1:
                    SpotLights[1].ToggleLight();
                    break;
                case 2:
                    //toggle point light
                    SpotLights[2].ToggleLight();
                    break;
                case 3:
                    PointLights[0].ToggleLight();
                    break;
                case 4:
                    PointLights[1].ToggleLight();
                    break;
                case 5:
                    DirectionalLight.ToggleLight();
                    SkyColor = Color4.CornflowerBlue;
                    break;
            }
        }
        
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