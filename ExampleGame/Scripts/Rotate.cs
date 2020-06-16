using Microsoft.Xna.Framework;
using Rander;
using Rander._2D;
using System;

namespace ExampleGame.Scripts
{
    class Rotate : Component2D
    {
        float MoveSpeed = 1;
        float MoveAmt = 100;

        public override void Update()
        {
            LinkedObject.Position = new Vector2((float)Math.Sin(Time.TimeSinceStart * MoveSpeed) * MoveAmt + Screen.Width / 2, Screen.Height / 2);
            LinkedObject.Rotation += Time.FrameTime * MoveSpeed;
        }
    }

    class NoRotate : Component2D
    {
        public override void Update()
        {
            LinkedObject.Rotation = 0;
            LinkedObject.Position = LinkedObject.Parent.Position + new Vector2(25, -25);
        }
    }
}
