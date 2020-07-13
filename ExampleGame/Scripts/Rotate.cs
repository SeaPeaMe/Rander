using Microsoft.Xna.Framework;
using Rander;
using Rander._2D;
using System;

namespace ExampleGame.Scripts
{
    class Rotate : Component2D
    {
        float MoveSpeed = 2;

        public override void Start()
        {
            //LinkedObject.Rotation = 50;
        }

        public override void Update()
        {
            LinkedObject.Rotation = (float)Math.Sin(Time.TimeSinceStart * MoveSpeed) * 10;
            LinkedObject.Size = new Vector2((float)Math.Cos(Time.TimeSinceStart * MoveSpeed) * 10 + 200, (float)Math.Cos(Time.TimeSinceStart * MoveSpeed) * 10 + 60);
        }
    }

    class NoRotate : Component2D
    {
        public override void Update()
        {
            LinkedObject.Rotation = 0;
            LinkedObject.Position = LinkedObject.Parent.Position + new Vector2(15, -10);
        }
    }
}
