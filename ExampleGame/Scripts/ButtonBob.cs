using Microsoft.Xna.Framework;
using Rander;
using Rander._2D;
using System;

namespace ExampleGame.Scripts
{
    class ButtonBob : Component2D
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

    class RelativeTranslation : Component2D
    {
        float MoveSpeed = 2;

        public override void Start()
        {
            //LinkedObject.Rotation = 50;
        }

        public override void Update()
        {
            LinkedObject.Rotation = Time.TimeSinceStart * MoveSpeed * 10;
        }
    }

    class TextNoRotate : Component2D
    {
        public override void Update()
        {
            LinkedObject.Position = LinkedObject.Parent.Position + new Vector2(LinkedObject.Parent.Size.X / 2, -LinkedObject.Parent.Size.X / 2);
            LinkedObject.Rotation = 0;
        }
    }

    class ObjectNoRotate : Component2D
    {
        public override void Update()
        {
            LinkedObject.Position = LinkedObject.Parent.Position + new Vector2((float)Math.Cos(Time.TimeSinceStart * 3) * 100, (float)Math.Sin(Time.TimeSinceStart * 3) * 100);
        }
    }

    class Fade : Component2D
    {
        public override void Start()
        {
            FadeStep();
        }

        void FadeStep()
        {
            Text2DComponent Col = LinkedObject.GetComponent<Text2DComponent>();
            if (Col.Color.A > 0)
            {
                Col.Color.A -= 1;
                Time.Wait(10, FadeStep);
            }
            else
            {
                Col.LinkedObject.Destroy();
            }
        }
    }
}
