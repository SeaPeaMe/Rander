using Rander._2D;
using Rander;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ExampleGame.Scripts
{
    class CursorReplacement : Component2D
    {
        float PulseMagnitude = 3;
        float PulseSpeed = 3;
        float BaseSize = 10;

        public int i;

        float Timer = 0;

        public override void Start()
        {
            MouseInput.IsVisible = false;
        }

        public override void Update()
        {
            LinkedObject.Position = MouseInput.Position.ToVector2();
            LinkedObject.Rotation += 15 * Time.FrameTime;
            LinkedObject.Size = new Vector2((float)Math.Sin(Time.TimeSinceStart * PulseSpeed) * PulseMagnitude + BaseSize + PulseMagnitude, (float)Math.Sin(Time.TimeSinceStart * PulseSpeed) * PulseMagnitude + BaseSize + PulseMagnitude);

            if (Time.TimeSinceStart > Timer && Input.Mouse.LeftButton == ButtonState.Pressed) {
                new Object2D("P" + i, LinkedObject.Position, new Vector2(10, 10), Rand.RandomFloat(0, 90), new Component2D[] { new Image2DComponent(DefaultValues.PixelTexture, Color.Blue), new Particle(i) }, Alignment.Center, 0.95f);
                i++;
                Timer += 0.01f;
            }
        }
    }
}
