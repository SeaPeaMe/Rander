/////////////////////////////////////
///          Base Script          ///
///          Use: Input           ///
///         Attatch: Base         ///
/////////////////////////////////////

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Rander
{
    public class Input : Component
    {

        public static KeyboardState Keys;
        public static MouseState Mouse;

        public override void Update()
        {
            Keys = Keyboard.GetState();
            Mouse = MouseInput.MouseInfo;
        }

        public static GamePadState Controller(PlayerIndex playerNumber)
        {
            return GamePad.GetState(playerNumber);
        }
    }
}
