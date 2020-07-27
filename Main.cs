/////////////////////////////////////
///         Main Loop/Vars        ///
///          Use: Global          ///
///         Attatch: N/A          ///
/////////////////////////////////////

using Microsoft.Xna.Framework;
using Rander;
using Rander._2D;
using Rander.BaseComponents;
using Rander.TestScripts;

namespace MyGame
{
    class Main
    {
        // Load game's resources and instantiate stuff here
        public static bool OnGameLoad()
        {
            Object2D Input = new Object2D("Input", new Vector2(Screen.Width / 2, Screen.Height / 2), new Vector2(300, 60), 0, new Component2D[] { new Image2DComponent(DefaultValues.PixelTexture), new Input2DComponent("Type name in here!", DefaultValues.DefaultFont, Color.DarkGray, Color.Black, Color.Black, 0.16f, 0.18f, Alignment.Center) }, Alignment.Center);
            new Object2D("Button", new Vector2(Screen.Width / 2, Screen.Height / 2 + 70), new Vector2(150, 30), 0, new Component2D[] { new Image2DComponent(DefaultValues.PixelTexture), new Button2DComponent(() => { if (Level.Object2DExists("Text")) { Level.FindObject2D("Text").Destroy(true); } Audio.PlaySound(ContentLoader.LoadSound("ExampleGameAssets/Boop"), 0, 1, false, true); new Object2D("Text", new Vector2(Screen.Width / 2, 0), new Vector2(300, 60), 0, new Component2D[] { new Text2DComponent("Hello, " + Input.GetComponent<Input2DComponent>().InputText + "!", DefaultValues.DefaultFont, Color.White, Alignment.TopCenter) }, Alignment.TopCenter); }), new Text2DComponent("Submit", DefaultValues.DefaultFont, Color.Black, 0.16f, Alignment.Center) }, Alignment.Center);
            new Object2D("CheckBox", new Vector2(Screen.Width / 2, Screen.Height / 2 + 140), new Vector2(30, 30), 0, new Component2D[] { new Checkbox2DComponent(Color.Red, Color.Green) }, Alignment.Center);
            //ExampleGame.Main.OnGameLoad(); // Runs the Example

            return true;
        }

        // Updates consistently (30TPS)
        public static void OnUpdate()
        {
            //ExampleGame.Main.OnUpdate(); // Updates the Example
        }

        // Updates inconsistently (Can go from 1-Infinity TPS), main use is for rendering things on-screen
        public static void OnDraw()
        {

        }
    }
}
