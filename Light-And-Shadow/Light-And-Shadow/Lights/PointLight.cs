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
    }
    
    public PointLight(World currentWorld, Color4 color, float intensity = 1)
    {
        Transform = new Transform();
        currentWorld.PointLights.Add(this);
        
        LightIntensity = intensity;
        LightColor = new Vector3(color.R * LightIntensity, color.G* LightIntensity, color.B* LightIntensity);
        DefaultColor = LightColor;
    }
}