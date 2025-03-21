using Light_And_Shadow.Behaviors;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Light_And_Shadow
{
    public class Camera : Behaviour
    {
        Vector3 front = new Vector3(0.0f, 0.0f, -1.0f);
        Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);
        float speed = 10;
        private float FOV;
        private float aspectX;
        private float aspectY;
        private float near;
        private float far;

        public Camera(GameObject gameObject, Game window, float FOV, float aspectX, float aspectY,float near,float far) : base(gameObject, window)
        {
            gameObject.Transform.Position = new Vector3(0.0f, 0.0f, 5.0f);
            this.FOV = FOV;
            this.aspectX = aspectX;
            this.aspectY = aspectY;
            this.near = near;
            this.far = far;
        }

        public override void Update(FrameEventArgs args)
        { 
            KeyboardState input = window.KeyboardState;
            if (input.IsKeyDown(Keys.W))
            {
                gameObject.Transform.Position += front * speed* (float)args.Time; //Forward 
            }
            if (input.IsKeyDown(Keys.S))
            {
                gameObject.Transform.Position -= front * speed* (float)args.Time; //Backwards
            }
            if (input.IsKeyDown(Keys.A))
            {
                gameObject.Transform.Position -= Vector3.Normalize(Vector3.Cross(front, up)) * speed* (float)args.Time; //Left
            }
            if (input.IsKeyDown(Keys.D))
            {
                gameObject.Transform.Position += Vector3.Normalize(Vector3.Cross(front, up)) * speed * (float)args.Time; //Right
            }
            if (input.IsKeyDown(Keys.Space))
            {
                gameObject.Transform.Position += up * speed * (float)args.Time; //Up 
            }

            if (input.IsKeyDown(Keys.LeftShift))
            {
                gameObject.Transform.Position -= up * speed * (float)args.Time; //Down
            }
        }
        public Matrix4 GetViewProjection()
        {
            Matrix4 view = Matrix4.LookAt(gameObject.Transform.Position, gameObject.Transform.Position + front, up);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(FOV), aspectX / aspectY, near, far);
            return view * projection;
        }
    }
}