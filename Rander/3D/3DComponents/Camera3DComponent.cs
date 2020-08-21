using Microsoft.Xna.Framework;

namespace Rander._3D._3DComponents
{
    public class Camera3DComponent : Component3D
    {
        public float FOV = 90;
        public Matrix ViewMatrix;
        public Matrix ProjectionMatrix;
        Matrix WorldMatrix;

        public Camera3DComponent(float fov = 90)
        {
            FOV = fov;

            if (Level.ActiveCamera == null)
            {
                Level.ActiveCamera = this;
            }
        }

        public override void Update()
        {
            WorldMatrix = LinkedObject.WorldMatrix;
            ViewMatrix = Matrix.CreateLookAt(LinkedObject.Position, Vector3.Forward, Vector3.Up);
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(FOV), Game.graphics.PreferredBackBufferWidth / Game.graphics.PreferredBackBufferHeight, 0.1f, 1000);
        }
    }
}
