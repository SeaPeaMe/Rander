using ExampleGame.Scripts;
using Microsoft.Xna.Framework;
using Rander;
using Rander._2D;
using Rander.TestScripts;

namespace ExampleGame
{
    class Main
    {
        static ExampleGame Game = ExampleGame.Particles;

        public static void OnGameLoad()
        {
            // Title
            new Object2D("Title", new Vector2(Screen.Width / 2, 20), Vector2.Zero, 0, new Component2D[] { new Text2DComponent(Game.ToString(), DefaultValues.DefaultFont, Color.White, 0.5f, Alignment.TopCenter) });
            // FPS
            new Object2D("FPSText", new Vector2(10, 5), new Vector2(100, 100), 0, new Component2D[] { new Text2DComponent("FPS", DefaultValues.DefaultFont, Color.Green, 0.18f), new FPSScript() }); // FPS Counter

            if (Game == ExampleGame.Particles) {
                new Object2D("Cursor", new Vector2(0, 0), new Vector2(10, 10), 0, new Component2D[] { new Image2DComponent(DefaultValues.PixelTexture, Color.White), new CursorReplacement() }, Alignment.Center, 1);
            } else if (Game == ExampleGame.Physics2D)
            {
                new Object2D("Obj1", new Vector2(Screen.Width / 2, 50), new Vector2(200, 200), 0, new Component2D[] { new Image2DComponent(DefaultValues.PixelTexture, Color.Blue), new Physics2DComponent() }, Alignment.TopCenter);
                new Object2D("Obj2", new Vector2(Screen.Width / 2, Screen.Height), new Vector2(Screen.Width, 100), 0, new Component2D[] { new Image2DComponent(DefaultValues.PixelTexture, Color.Red), new Physics2DComponent(false) }, Alignment.BottomCenter);
            } else if (Game == ExampleGame.ChildTest)
            {
                Object2D Parent = new Object2D("Parent", new Vector2(Screen.Width / 2, Screen.Height / 2), new Vector2(20, 20), 0, new Component2D[] { new Image2DComponent(DefaultValues.PixelTexture, Color.Turquoise), new Rotate() }, Alignment.Center);
                new Object2D("ParentText", new Vector2(Screen.Width / 2 + 20, Screen.Height / 2 + 20), new Vector2(20, 20), 0, new Component2D[] { new Text2DComponent("Parent", DefaultValues.DefaultFont, Color.Turquoise, 0.18f, Alignment.BottomLeft), new NoRotate() }, Alignment.BottomLeft, 1, Parent);
                Object2D Child = new Object2D("Child", new Vector2(Screen.Width / 2 + 150, Screen.Height / 2 + 130), Parent.Size * 3, 0, new Component2D[] { new Image2DComponent(DefaultValues.PixelTexture, Color.LightBlue) }, Alignment.Center, 0.5f, Parent);
                new Object2D("ChildText", new Vector2(Child.Position.X + 20, Child.Position.Y + 20), new Vector2(20, 20), 0, new Component2D[] { new Text2DComponent("Child", DefaultValues.DefaultFont, Color.LightBlue, 0.18f, Alignment.BottomLeft), new NoRotate() }, Alignment.BottomLeft, 1, Child);
            }
        }

        public static void OnUpdate()
        {

        }
    }

    enum ExampleGame
    {
        Particles = 0,
        Physics2D = 1,
        ChildTest = 2
    }
}