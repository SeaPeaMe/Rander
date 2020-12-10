using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rander._2D
{
    public class Camera2DComponent : Component2D
    {
        public Matrix Matrix;

        public override void Awake()
        {
            if (Level.Active2DCamera == null) Level.Active2DCamera = this;
        }

        public override void Update()
        {
            Matrix = Matrix.Identity * Matrix.CreateRotationZ(MathHelper.ToRadians(LinkedObject.Rotation)) * Matrix.CreateTranslation(new Vector3(-LinkedObject.Position.X, -LinkedObject.Position.Y, 0)) * Matrix.CreateScale(LinkedObject.Size.X, LinkedObject.Size.Y, 1);
        }
    }
}
