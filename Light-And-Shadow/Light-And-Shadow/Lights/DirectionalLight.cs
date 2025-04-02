using Light_And_Shadow.Materials;
using Light_And_Shadow.Worlds;
using OpenTK_OBJ;
using OpenTK.Mathematics;

namespace Light_And_Shadow.Lights;

public class DirectionalLight : Light
{
    public Transform Transform;

    public DirectionalLight(World currentWorld)
    {
        Transform = new Transform();
        currentWorld.DirectionalLight = this;
        
        CreateVisualizer(currentWorld);
    }

    public DirectionalLight(World currentWorld, Color4 color, float intensity = 1)
    {
        Transform = new Transform();
        currentWorld.DirectionalLight = this;
        
        LightIntensity = intensity;
        LightColor = new Vector3(color.R * LightIntensity, color.G* LightIntensity, color.B* LightIntensity);
        DefaultColor = LightColor;
        
        CreateVisualizer(currentWorld);
    }

    private void CreateVisualizer(World currentWorld)
    {
        Visualizer = new GameObjectBuilder(currentWorld.Game)
            .Model("Arrow")
            .Material(new mat_default())
            .Position(0, 2, 0)
            .Scale(0.1f, 0.1f, 0.1f)
            .Build();

        // currentWorld.GameObjects.Add(Visualizer);
        
        Visualizer.Transform.Position = new Vector3(0, 2, 0);
        Visualizer.Transform.Rotation = Transform.Rotation;
    }

    public void UpdateVisualizer(World currentWorld)
    {
        if (Visualizer != null) Visualizer.Transform.Rotation = Transform.Rotation;
    }
}