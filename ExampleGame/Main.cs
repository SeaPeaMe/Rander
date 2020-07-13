using ExampleGame.Scripts;
using Microsoft.Xna.Framework;
using Rander;
using Rander._2D;
using Rander.TestScripts;
using System.IO;
using System;
using System.Runtime.CompilerServices;

namespace ExampleGame
{
    class Main
    {
        static ExampleGame Game = ExampleGame.UI;

        public static void OnGameLoad()
        {
            if (File.Exists(DefaultValues.ExecutableFolderPath + "/RunSettings.txt"))
            {
                Game = (ExampleGame)Enum.Parse(typeof(ExampleGame), File.ReadAllText(DefaultValues.ExecutableFolderPath + "/RunSettings.txt")); 
            }

            // Title
            new Object2D("Title", new Vector2(Screen.Width / 2, 20), Vector2.Zero, 0, new Component2D[] { new Text2DComponent(Game.ToString(), DefaultValues.DefaultFont, Color.White, 0.5f, Alignment.TopCenter) }, Alignment.TopCenter);
            // FPS
            new Object2D("FPSText", new Vector2(10, 5), new Vector2(100, 100), 0, new Component2D[] { new Text2DComponent("FPS", DefaultValues.DefaultFont, Color.Green, 0.18f), new FPSScript() }); // FPS Counter

            switch(Game)
            {
                case ExampleGame.Particles:
                    new Object2D("Cursor", new Vector2(0, 0), new Vector2(10, 10), 0, new Component2D[] { new Image2DComponent(DefaultValues.PixelTexture, Color.White), new CursorReplacement() }, Alignment.Center, 1);
                    break;
                case ExampleGame.RelativeTranslation:
                    Object2D Parent = new Object2D("Parent", new Vector2(Screen.Width / 2, Screen.Height / 2), new Vector2(20, 20), 0, new Component2D[] { new Image2DComponent(DefaultValues.PixelTexture, Color.Turquoise), new Rotate() }, Alignment.Center);
                    new Object2D("ParentText", new Vector2(Screen.Width / 2 + 20, Screen.Height / 2 + 20), new Vector2(20, 20), 0, new Component2D[] { new Text2DComponent("Parent", DefaultValues.DefaultFont, Color.Turquoise, 0.18f, Alignment.BottomLeft), new NoRotate() }, Alignment.BottomLeft, 1, Parent);
                    Object2D Child = new Object2D("Child", new Vector2(Screen.Width / 2 + 150, Screen.Height / 2 + 130), Parent.Size * 3, 0, new Component2D[] { new Image2DComponent(DefaultValues.PixelTexture, Color.LightBlue) }, Alignment.Center, 0.5f, Parent);
                    new Object2D("ChildText", new Vector2(Child.Position.X + 20, Child.Position.Y + 20), new Vector2(20, 20), 0, new Component2D[] { new Text2DComponent("Child", DefaultValues.DefaultFont, Color.LightBlue, 0.18f, Alignment.BottomLeft), new NoRotate() }, Alignment.BottomLeft, 1, Child);
                    break;
                case ExampleGame.UI:
                    new Object2D("Button", new Vector2(Screen.Width / 2, Screen.Height / 2), new Vector2(200, 60), 0, new Component2D[] { new Button2DComponent(new Action(ButtonClicked), new Action(() => Object2D.Find("Button").GetComponent<Image2DComponent>().Color = Color.Red), null, null, new Action(() => Object2D.Find("Button").GetComponent<Image2DComponent>().Color = Color.Red), new Action(() => Object2D.Find("Button").GetComponent<Image2DComponent>().Color = Color.White)), new Image2DComponent(DefaultValues.PixelTexture), new Text2DComponent("Click Me!", DefaultValues.DefaultFont, Color.Black, 0.18f, Alignment.Center), new Rotate() }, Alignment.Center);
                    break;
            }
        }

        static int i = 0;
        static void ButtonClicked()
        {
            Object2D.Find("Button").GetComponent<Image2DComponent>().Color = Color.LightPink;

            Object2D Btn = new Object2D("SomeButton" + i, new Vector2(Rand.RandomInt(0, Screen.Width), Rand.RandomInt(0, Screen.Height)), new Vector2(100, 30), 0, new Component2D[] { new Text2DComponent("Button Was Clicked!", DefaultValues.DefaultFont, Color.White, Alignment.Center) });
            i++;
            Time.Wait(5000, () => Btn.Destroy(true));
        }

        public static void OnUpdate()
        {
            //Object2D.Find("Button").Size = new Vector2((float)Math.Sin(Time.TimeSinceStart) * 50 + 100, (float)Math.Cos(Time.TimeSinceStart) * 30 + 50);
        }
    }

    enum ExampleGame
    {
        Particles = 0,
        RelativeTranslation = 2,
        UI = 3
    }
}