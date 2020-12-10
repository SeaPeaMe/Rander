using Rander._2D;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rander.Examples
{
    class Rotate : Component2D
    {
        public override void FixedUpdate()
        {
            LinkedObject.Rotation = (float)Time.TimeSinceStart * 15;
        }
    }
}
