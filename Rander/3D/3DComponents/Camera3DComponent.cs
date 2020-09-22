using Microsoft.Xna.Framework;

namespace Rander._3D
{
    public class Camera3DComponent : Component3D
    {
        public float FOV = 75;
        public Matrix ViewMatrix;
        public Matrix ProjectionMatrix;

        public Camera3DComponent(float fov = 75)
        {
            FOV = fov;

            if (Level.ActiveCamera == null)
            {
                Level.ActiveCamera = this;
            }
        }

        public override void Start()
        {
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(FOV), Game.graphics.GraphicsDevice.DisplayMode.AspectRatio, 0.1f, 1000);
        }

        public override void Update()
        {
            ViewMatrix = Matrix.CreateLookAt(LinkedObject.Position, LinkedObject.Position + LinkedObject.Forward, Vector3.Up);
        }
    }
}
