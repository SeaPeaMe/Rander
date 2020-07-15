using ExampleGame.Scripts;
using Microsoft.Xna.Framework;
using Rander;
using Rander._2D;
using Rander.BaseComponents;
using Rander.TestScripts;
using System;
using System.IO;

namespace ExampleGame
{
    class Main
    {
        static ExampleGame ExGame = ExampleGame.Sounds;

        public static void OnGameLoad()
        {
            if (File.Exists(DefaultValues.ExecutableFolderPath + "/RunSettings.txt"))
            {
                ExGame = (ExampleGame)Enum.Parse(typeof(ExampleGame), File.ReadAllText(DefaultValues.ExecutableFolderPath + "/RunSettings.txt"));
            }

            // Title
            Object2D Title = new Object2D("Title", new Vector2(Screen.Width / 2, 20), Vector2.Zero, 0, new Component2D[] { new Text2DComponent(ExGame.ToString(), DefaultValues.DefaultFont, Color.White, 0.5f, Alignment.TopCenter) }, Alignment.TopCenter);
            // FPS
            new Object2D("FPSText", new Vector2(10, 5), new Vector2(100, 100), 0, new Component2D[] { new Text2DComponent("FPS", DefaultValues.DefaultFont, Color.LightGreen, 0.18f), new FPSScript() }); // FPS Counter

            switch (ExGame)
            {
                case ExampleGame.RelativeTranslation:
                    Rander.Game.BackgroundColor = Color.Black;
                    for (int i = 0; i < 2000; i++)
                    {
                        new Object2D("Star_" + i, new Vector2(Rand.RandomFloat(0, Screen.Width), Rand.RandomFloat(0, Screen.Height)), new Vector2(Rand.RandomFloat(1, 3)), 0, new Component2D[] { new Image2DComponent(DefaultValues.PixelTexture) }, Alignment.Center);
                    }

                    Object2D Sun = new Object2D("Parent", new Vector2(Screen.Width / 2, Screen.Height / 2), new Vector2(20, 20), 0, new Component2D[] { new Image2DComponent(ContentLoader.LoadTexture("ExampleGameAssets/Sun.png")), new RelativeTranslation() }, Alignment.Center, 0.1f);
                    new Object2D("ParentText", new Vector2(Screen.Width / 2 + 20, Screen.Height / 2 + 20), new Vector2(20, 20), 0, new Component2D[] { new Text2DComponent("Parent", DefaultValues.DefaultFont, Color.Yellow, 0.18f, Alignment.BottomLeft), new TextNoRotate() }, Alignment.BottomLeft, 1, Sun);
                    Object2D Earth = new Object2D("Child", new Vector2(Screen.Width / 2 + 150, Screen.Height / 2 + 130), Sun.Size * 3, 0, new Component2D[] { new Image2DComponent(ContentLoader.LoadTexture("ExampleGameAssets/Earth.png")) }, Alignment.Center, 0.1f, Sun);
                    new Object2D("ChildText", Earth.Position + new Vector2(25, 25), new Vector2(20, 20), 0, new Component2D[] { new Text2DComponent("Child of Parent", DefaultValues.DefaultFont, Color.LightBlue, 0.18f, Alignment.BottomLeft), new TextNoRotate() }, Alignment.BottomLeft, 1, Earth);
                    Object2D Moon = new Object2D("Child Child", Earth.Position, new Vector2(20, 20), 0, new Component2D[] { new Image2DComponent(ContentLoader.LoadTexture("ExampleGameAssets/Moon.png")), new ObjectNoRotate() }, Alignment.Center, 0.1f, Earth);
                    new Object2D("ChildChildText", Earth.Position + new Vector2(25, 25), new Vector2(20, 20), 0, new Component2D[] { new Text2DComponent("Child of Child", DefaultValues.DefaultFont, Color.White, 0.18f, Alignment.BottomLeft), new TextNoRotate() }, Alignment.BottomLeft, 1, Moon);
                    break;
                case ExampleGame.UI:
                    new Object2D("Button", new Vector2(Screen.Width / 2, Screen.Height / 2), new Vector2(200, 60), 0, new Component2D[] { new Button2DComponent(onClick: new Action(ButtonClicked), onRelease: new Action(() => Object2D.Find("Button").GetComponent<Image2DComponent>().Color = Color.Red), onEnter: new Action(() => Object2D.Find("Button").GetComponent<Image2DComponent>().Color = Color.Red), onExit: new Action(() => Object2D.Find("Button").GetComponent<Image2DComponent>().Color = Color.White)), new Image2DComponent(DefaultValues.PixelTexture), new Text2DComponent("Click Me!", DefaultValues.DefaultFont, Color.Black, 0.18f, Alignment.Center), new ButtonBob() }, Alignment.Center);
                    break;
                case ExampleGame.Sounds:
                    new Object2D("Disclaimer", new Vector2(Screen.Width / 2, 100), Vector2.Zero, 0, new Component2D[] { new Text2DComponent("(And yes, I know this was kinda in the UI example)", DefaultValues.DefaultFont, Color.White, 0.18f, Alignment.TopCenter) }, Alignment.TopCenter, parent: Title);
                    new Object2D("SoundButton", new Vector2(Screen.Width / 2, Screen.Height / 2), new Vector2(200, 60), 0, new Component2D[] { new Button2DComponent(onClick: () => { Audio.PlaySound(ContentLoader.LoadSound("ExampleGameAssets/Test")); }), new Image2DComponent(DefaultValues.PixelTexture), new Text2DComponent("Click me!", DefaultValues.DefaultFont, Color.Green, 0.18f, Alignment.Center) }, Alignment.Center);
                    break;
            }
        }

        static int i = 0;
        static void ButtonClicked()
        {
            Object2D.Find("Button").GetComponent<Image2DComponent>().Color = Color.LightPink;

            new Object2D("SomeButton" + i, new Vector2(Rand.RandomInt(0, Screen.Width), Rand.RandomInt(0, Screen.Height)), new Vector2(200, 60), 0, new Component2D[] { new Text2DComponent("Button Was Clicked!", DefaultValues.DefaultFont, Color.White, Alignment.Center), new Fade() }, Alignment.Center, 1);
            i++;
            Audio.PlaySound(ContentLoader.LoadSound("ExampleGameAssets/Boop"));
        }

        public static void OnUpdate()
        {

        }
    }

    enum ExampleGame
    {
        RelativeTranslation = 1,
        UI = 2,
        Sounds = 3
    }
}