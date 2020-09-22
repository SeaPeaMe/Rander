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
            //Rander.Editor.Main.Init();

            //Rander.Examples.FirstPerson3D.Start();
            Rander.Examples.Test.Start();

            return true;
        }

        // Updates consistently (30TPS)
        public static void OnUpdate()
        {
            //Rander.Editor.Main.Update();

            //Rander.Examples.FirstPerson3D.Update();
            Rander.Examples.Test.Update();
        }

        // Updates inconsistently (Can go from 1-Infinity TPS), main use is for rendering things on-screen
        public static void OnDraw()
        {

        }
    }
}
