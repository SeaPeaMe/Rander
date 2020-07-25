/////////////////////////////////////
///         Main Loop/Vars        ///
///          Use: Global          ///
///         Attatch: N/A          ///
/////////////////////////////////////

using Microsoft.Xna.Framework;
using Rander;
using Rander._2D;
using Rander._2D._2DComponents;
using Rander.TestScripts;

namespace MyGame
{
    class Main
    {
        // Load game's resources and instantiate stuff here
        public static bool OnGameLoad()
        {
            new Object2D("Input", new Vector2(Screen.Width / 2, Screen.Height / 2), new Vector2(300, 60), 0, new Component2D[] { new Image2DComponent(DefaultValues.PixelTexture), new Input2DComponent("Click Me!", DefaultValues.DefaultFont, Color.DarkGray, Color.Black, Color.Black, 0.18f) });
            //ExampleGame.Main.OnGameLoad(); // Runs the Example

            return true;
        }

        // Updates consistently (30TPS)
        public static void OnUpdate()
        {
            ExampleGame.Main.OnUpdate(); // Updates the Example
        }

        // Updates inconsistently (Can go from 1-Infinity TPS), main use is for rendering things on-screen
        public static void OnDraw()
        {

        }
    }
}
