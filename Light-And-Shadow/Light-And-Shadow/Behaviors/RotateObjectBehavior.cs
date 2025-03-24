using OpenTK.Windowing.Common;

namespace Light_And_Shadow.Behaviors;

public class RotateObjectBehavior : Behaviour
{
    private float rotateSpeed = 1.0f;

    public RotateObjectBehavior(GameObject gameObject, Game window) : base(gameObject, window)
    {
    }

    public override void Update(FrameEventArgs args)
    {
        var rot = gameObject.Transform.Rotation; // get current position

        rot.X += rotateSpeed * (float)args.Time;
        
        gameObject.Transform.Rotation = rot;
    }
}