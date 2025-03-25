using Light_And_Shadow.Worlds;
using OpenTK_OBJ;
using OpenTK.Mathematics;

namespace Light_And_Shadow;

public class DirectionalLight : Light
{
    public Transform Transform;

    public DirectionalLight(World currentWorld)
    {
        Transform = new Transform();
        currentWorld.DirectionalLight = this;
    }

    public DirectionalLight(World currentWorld, Color4 color, float intensity = 1)
    {
        Transform = new Transform();
        currentWorld.DirectionalLight = this;
        
        LightIntensity = intensity;
        LightColor = new Vector3(color.R * LightIntensity, color.G* LightIntensity, color.B* LightIntensity);
        DefaultColor = LightColor;
    }
}