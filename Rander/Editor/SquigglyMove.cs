using Microsoft.Xna.Framework;
using Rander._2D;
using System;

namespace Rander.Editor
{
    class SquigglyMove : Component2D
    {
        float Offset = Rand.RandomFloat(0, 360);
        Vector2 StartPos;

        public override void Start()
        {
            StartPos = LinkedObject.Position;
            LinkedObject.AddComponent(new Button2DComponent(onEnter: () => { LinkedObject.Dispose(); }));
            LinkedObject.Position = StartPos;
        }

        public override void Update()
        {
            LinkedObject.Rotation = (float)Math.Sin(Time.TimeSinceStart * 10 - Offset) * 25;
        }
    }
}
