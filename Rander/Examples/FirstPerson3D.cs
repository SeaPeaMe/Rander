using Assimp;
using Microsoft.Xna.Framework;
using Rander._3D;

namespace Rander.Examples
{
    class FirstPerson3D
    {
        static Object3D Cam;
        static Object3D Cube;
        public static void Start()
        {
            Cam = new Object3D("Camera", new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Component3D[] { new Camera3DComponent(75) });

            AssimpContext imp = new AssimpContext();
            Scene scn = imp.ImportFile(ContentLoader.ContentPath + "/Defaults/Cube.dae");

            Cube = new Object3D("Cube", new Vector3(0, 0, 5), new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Component3D[] { new Renderer3DComponent(scn.Meshes[0], new Vector3(0.1f, 0.1f, 0.1f), new Vector3(1, 0, 0)) });
        }

        public static void Update()
        {
            float lookSpeed = 10;
            float MoveSpeed = 10;

            if (Input.Keys.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
            {
                Cam.Position += Cam.Forward * Time.FrameTime * MoveSpeed;
            }
            if (Input.Keys.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
            {
                Cam.Position += Cam.Left * Time.FrameTime * MoveSpeed;
            }
            if (Input.Keys.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
            {
                Cam.Position -= Cam.Forward * Time.FrameTime * MoveSpeed;
            }
            if (Input.Keys.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
            {
                Cam.Position -= Cam.Left * Time.FrameTime * MoveSpeed;
            }

            if (Input.Keys.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
            {
                Cam.Rotation += new Vector3(0, 20 * Time.FrameTime * lookSpeed, 0);
            }
            if (Input.Keys.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
            {
                Cam.Rotation -= new Vector3(0, 20 * Time.FrameTime * lookSpeed, 0);
            }
            if (Input.Keys.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
            {
                Cam.Rotation -= new Vector3(20 * Time.FrameTime * lookSpeed, 0, 0);
            }
            if (Input.Keys.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
            {
                Cam.Rotation += new Vector3(20 * Time.FrameTime * lookSpeed, 0, 0);
            }
        }
    }
}
