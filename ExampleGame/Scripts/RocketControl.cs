using Microsoft.Xna.Framework;
using Rander;
using Rander._2D;
using Rander.BaseComponents;

namespace ExampleGame.Scripts
{
    class RocketControlVertical : Component2D
    {
        static bool LockControls = false;
        public static bool CtrlLck { get { return LockControls; } set { LockControls = value; if (!value) { Audio.StopAllAudio(); } } }

        Vector2 Velocity = Vector2.Zero;

        float ShakeMag = 1;

        public override void Start()
        {
            Shake();
        }

        public override void Update()
        {
            if (!LockControls)
            {
                if (Input.Keys.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
                {
                    Velocity -= new Vector2(0, 1);
                    MenuStarMove.StarMoveSpeed = -Velocity * 5;
                    ShakeMag += 3 * Time.FrameTime;
                    LinkedObject.GetComponent<Image2DComponent>().Texture = ContentLoader.LoadTexture("ExampleGameAssets/Rocket_2.png");
                }
                else
                {
                    LinkedObject.GetComponent<Image2DComponent>().Texture = ContentLoader.LoadTexture("ExampleGameAssets/Rocket_1.png");
                    if (ShakeMag > 0)
                    {
                        ShakeMag -= 5 * Time.FrameTime;
                    }
                    else if (ShakeMag < 0)
                    {
                        ShakeMag = 0;
                    }
                }

                LinkedObject.Position += Velocity * Time.FrameTime;

                Audio.PlaySound(ContentLoader.LoadSound("ExampleGameAssets/RocketRumble"), ShakeMag / 50, MathHelper.Clamp(ShakeMag / 5, 0, 1));
            }
        }

        Vector2 PrevMag = Vector2.Zero;
        void Shake()
        {
            Vector2 NewMag = new Vector2(Rand.RandomFloat(-ShakeMag, ShakeMag), Rand.RandomFloat(-ShakeMag, ShakeMag));
            LinkedObject.Position -= PrevMag;
            LinkedObject.Position += NewMag;
            PrevMag = NewMag;
            Time.Wait(10, Shake);
        }
    }

    class RocketControlFree : Component2D
    {
        Vector2 Velocity = Vector2.Zero;

        float ShakeMag = 1;

        public override void Start()
        {
            Shake();
        }

        public override void Update()
        {
            if (Input.Keys.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
            {
                LinkedObject.Rotation = 0;
                Velocity -= new Vector2(0, 1);
                MenuStarMove.StarMoveSpeed = -Velocity * 5;
                ShakeMag += 2 * Time.FrameTime;
                LinkedObject.GetComponent<Image2DComponent>().Texture = ContentLoader.LoadTexture("ExampleGameAssets/Rocket_2.png");
            } else if (Input.Keys.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
            {
                LinkedObject.Rotation = -90;
                Velocity -= new Vector2(1, 0);
                MenuStarMove.StarMoveSpeed = -Velocity * 5;
                ShakeMag += 2 * Time.FrameTime;
                LinkedObject.GetComponent<Image2DComponent>().Texture = ContentLoader.LoadTexture("ExampleGameAssets/Rocket_2.png");
            } else if (Input.Keys.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
            {
                LinkedObject.Rotation = 180;
                Velocity -= new Vector2(0, -1);
                MenuStarMove.StarMoveSpeed = -Velocity * 5;
                ShakeMag += 2 * Time.FrameTime;
                LinkedObject.GetComponent<Image2DComponent>().Texture = ContentLoader.LoadTexture("ExampleGameAssets/Rocket_2.png");
            } else if (Input.Keys.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
            {
                LinkedObject.Rotation = 90;
                Velocity -= new Vector2(-1, 0);
                MenuStarMove.StarMoveSpeed = -Velocity * 5;
                ShakeMag += 2 * Time.FrameTime;
                LinkedObject.GetComponent<Image2DComponent>().Texture = ContentLoader.LoadTexture("ExampleGameAssets/Rocket_2.png");
            }
            else
            {
                LinkedObject.GetComponent<Image2DComponent>().Texture = ContentLoader.LoadTexture("ExampleGameAssets/Rocket_1.png");
                if (ShakeMag > 0)
                {
                    ShakeMag -= 5 * Time.FrameTime;
                }
                else if (ShakeMag < 0)
                {
                    ShakeMag = 0;
                }

                if (Velocity.Length() > 0)
                {
                    Velocity *= 55 * Time.FrameTime;
                    MenuStarMove.StarMoveSpeed = -Velocity * 5;
                }
            }

            Audio.PlaySound(ContentLoader.LoadSound("ExampleGameAssets/RocketRumble"), ShakeMag / 50, MathHelper.Clamp(ShakeMag / 5, 0, 1));
        }

        Vector2 PrevMag = Vector2.Zero;
        void Shake()
        {
            Vector2 NewMag = new Vector2(Rand.RandomFloat(-ShakeMag, ShakeMag), Rand.RandomFloat(-ShakeMag, ShakeMag));
            LinkedObject.Position -= PrevMag;
            LinkedObject.Position += NewMag;
            PrevMag = NewMag;
            Time.Wait(10, Shake);
        }
    }
}
