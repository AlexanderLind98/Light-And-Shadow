using Light_And_Shadow.Materials;
using Light_And_Shadow.Worlds;
using OpenTK_OBJ;
using OpenTK.Audio.OpenAL;
using OpenTK.Mathematics;

namespace Light_And_Shadow.Lights;

public class SpotLight : Light
{
    public Transform Transform;
    public float InnerRadius = 12.5f;
    public float OuterRadius = 15.5f;
    
    public SpotLight(World currentWorld)
    {
        Transform = new Transform();
        currentWorld.SpotLights.Add(this);
        
        CreateVisualizer(currentWorld);
    }
    
    public SpotLight(World currentWorld, Color4 color, float intensity = 1, float innerRadius = 12.5f, float outerRadius = 15.5f)
    {
        Transform = new Transform();
        currentWorld.SpotLights.Add(this);
        
        LightIntensity = intensity;
        LightColor = new Vector3(color.R * LightIntensity, color.G* LightIntensity, color.B* LightIntensity);
        DefaultColor = LightColor;
        
        CreateVisualizer(currentWorld);
    }
    
    public SpotLight(World currentWorld, Color4 color, Vector3 position, Vector3 rotation, float intensity = 1, float innerRadius = 12.5f, float outerRadius = 15.5f)
    {
        Transform = new Transform
        {
            Position = position,
            Rotation = rotation
        };

        currentWorld.SpotLights.Add(this);
        
        LightIntensity = intensity;
        LightColor = new Vector3(color.R * LightIntensity, color.G* LightIntensity, color.B* LightIntensity);
        DefaultColor = LightColor;
        
        CreateVisualizer(currentWorld);
    }

    public SpotLight(World currentWorld, Color4 color, Transform transform, float intensity = 1, float innerRadius = 12.5f, float outerRadius = 15.5f)
    {
        Transform = transform;
        currentWorld.SpotLights.Add(this);
        
        LightIntensity = intensity;
        LightColor = new Vector3(color.R * LightIntensity, color.G* LightIntensity, color.B* LightIntensity);
        DefaultColor = LightColor;
        
        CreateVisualizer(currentWorld);
    }
    
    private void CreateVisualizer(World currentWorld)
    {
        Vector3 Direction = ForwardFromEuler(Transform.Rotation);

        if (currentWorld.SpotLights.Count == 1)
            return;

        Visualizer = new GameObjectBuilder(currentWorld.Game)
            .Model("Arrow")
            .Material(new mat_concrete())
            .Position(0, 0, 0)
            .Scale(0.1f, 0.1f, 0.1f)
            .Build();

        currentWorld.GameObjects.Add(Visualizer);

        // ✅ Correctly rotate the arrow to match the converted direction
        Visualizer.Transform.Position = Transform.Position;
        Visualizer.Transform.Rotation = new Vector3(Direction.X, Direction.Y, Direction.Z);
        
        Console.WriteLine($"Spotlight Rotation (Radians): {Transform.Rotation}");
        Console.WriteLine($"Spotlight Forward Direction: {Direction}");
        Console.WriteLine($"Visualizer Rotation (Radians): {Visualizer.Transform.Rotation}");
    }
    
    public void UpdateVisualizer(World currentWorld)
    {
        if (Visualizer != null)
        {
            Visualizer.Transform.Position = Transform.Position;
            Visualizer.Transform.Rotation = Transform.Rotation;
        }
    }
}