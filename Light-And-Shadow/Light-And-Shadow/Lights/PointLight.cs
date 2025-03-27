using Light_And_Shadow.Materials;
using Light_And_Shadow.Worlds;
using OpenTK_OBJ;
using OpenTK.Mathematics;

namespace Light_And_Shadow.Lights;

public class PointLight : Light
{
    public Transform Transform;
    
    public PointLight(World currentWorld)
    {
        Transform = new Transform();
        currentWorld.PointLights.Add(this);
        
        Visualizer = new GameObjectBuilder(currentWorld.Game)
            .Model("Sphere")
            .Material(new mat_concrete())
            .Position(Transform.Position.X, Transform.Position.Y, Transform.Position.Z)
            .Scale(0.1f, 0.1f, 0.1f)
            .Build();

        currentWorld.GameObjects.Add(Visualizer);
    }
    
    public PointLight(World currentWorld, Color4 color, float intensity = 1)
    {
        Transform = new Transform();
        currentWorld.PointLights.Add(this);
        
        LightIntensity = intensity;
        LightColor = new Vector3(color.R * LightIntensity, color.G* LightIntensity, color.B* LightIntensity);
        DefaultColor = LightColor;
        
        Visualizer = new GameObjectBuilder(currentWorld.Game)
            .Model("Sphere")
            .Material(new mat_concrete())
            .Position(Transform.Position.X, Transform.Position.Y, Transform.Position.Z)
            .Scale(0.1f, 0.1f, 0.1f)
            .Build();

        currentWorld.GameObjects.Add(Visualizer);
    }
    
    public void UpdateVisualizer(World currentWorld)
    {
        if (Visualizer != null) Visualizer.Transform.Position = Transform.Position;
    }
}