using System.Drawing;
using OpenTK.Mathematics;

namespace Light_And_Shadow;

public class Light
{
    public Vector3 LightColor { get; set; } = Vector3.One;
    protected Vector3 DefaultColor = Vector3.One;
    protected float LightIntensity { get; set; } = 1.0f;
    
    public float Constant = 1.0f;
    public float Linear = 0.09f;
    public float Quadratic = 0.032f;

    public void ToggleLight()
    {
        LightColor = LightColor == DefaultColor ? Vector3.Zero : DefaultColor;
    }
}