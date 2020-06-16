﻿/////////////////////////////////////
///          Base Script          ///
///          Use: Input           ///
///         Attatch: Base         ///
/////////////////////////////////////

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
    }
}
