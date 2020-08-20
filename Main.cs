/////////////////////////////////////
///         Main Loop/Vars        ///
///          Use: Global          ///
///         Attatch: N/A          ///
/////////////////////////////////////

namespace MyGame
{
    class Main
    {
        // Load game's resources and instantiate stuff here
        public static bool OnGameLoad()
        {
            Rander.Editor.Main.Init();

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
