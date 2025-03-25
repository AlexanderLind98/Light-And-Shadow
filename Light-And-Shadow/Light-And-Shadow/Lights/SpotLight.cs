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
    }
    
    public SpotLight(World currentWorld, Color4 color, float intensity = 1, float innerRadius = 12.5f, float outerRadius = 15.5f)
    {
        Transform = new Transform();
        currentWorld.SpotLights.Add(this);
        
        LightIntensity = intensity;
        LightColor = new Vector3(color.R * LightIntensity, color.G* LightIntensity, color.B* LightIntensity);
        DefaultColor = LightColor;
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
    }
    
    public SpotLight(World currentWorld, Color4 color, Transform transform, float intensity = 1, float innerRadius = 12.5f, float outerRadius = 15.5f)
    {
        Transform = transform;
        currentWorld.SpotLights.Add(this);
        
        LightIntensity = intensity;
        LightColor = new Vector3(color.R * LightIntensity, color.G* LightIntensity, color.B* LightIntensity);
        DefaultColor = LightColor;
    }
}