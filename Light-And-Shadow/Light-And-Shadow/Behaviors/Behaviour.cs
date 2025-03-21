using OpenTK.Windowing.Common;

namespace Light_And_Shadow.Behaviors
{
    public abstract class Behaviour
    {
        protected GameObject gameObject;
        protected Game window; 
        public Behaviour(GameObject gameObject, Game window)
        {
            this.gameObject = gameObject;
            this.window = window;
        }

        public abstract void Update(FrameEventArgs args);
    }
}