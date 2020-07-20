using Microsoft.Xna.Framework;
using Rander;
using Rander._2D;

namespace ExampleGame.Scripts
{
    class MenuStarMove : Component2D
    {
        public static Vector2 StarMoveSpeed = new Vector2(0, 10);

        public override void Update()
        {
            LinkedObject.Position += StarMoveSpeed * Time.FrameTime;

            if (LinkedObject.Position.Y > Screen.Height)
            {
                LinkedObject.Position -= new Vector2(0, Screen.Height);
            } else if (LinkedObject.Position.Y < 0)
            {
                LinkedObject.Position += new Vector2(0, Screen.Height);
            }

            if (LinkedObject.Position.X > Screen.Width)
            {
                LinkedObject.Position -= new Vector2(Screen.Width, 0);
            } else if (LinkedObject.Position.X < 0)
            {
                LinkedObject.Position += new Vector2(Screen.Width, 0);
            }
        }
    }
}
