/////////////////////////////////////
///          Base Script          ///
///          Use: Input           ///
///         Attatch: Base         ///
/////////////////////////////////////

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace Rander
{
    public class Input : BaseScript
    {

        public static KeyboardState Keys;
        public static MouseState Mouse;
        public static List<Keybind> Keybinds = new List<Keybind>();

        public override void Update()
        {
            Keys = Keyboard.GetState();
            Mouse = MouseInput.MouseInfo;
            foreach (Keybind kb in Keybinds)
            {
                kb.KeysPressed = new List<bool>();
                foreach (Keys key in kb.KeybindKeys)
                {
                    if (Keys.IsKeyDown(key))
                    {
                        kb.KeysPressed.Add(true);
                    }
                }

                foreach (Buttons btn in kb.KeybindGamepadButtons)
                {
                    if (Controller(kb.PlayerID).IsButtonDown(btn))
                    {
                        kb.KeysPressed.Add(true);
                    }
                }

                // Checks if the button was pressed at the start to give the program a bit of time to do checks on the states
                if (kb.CurrentState == KeybindState.Pressed)
                {
                    Keybind bind = Keybinds.First((x) => x.KeybindName == kb.KeybindName);
                    bind.CurrentState = KeybindState.Held;
                    kb.WasPressed = true;
                }
                else if (kb.CurrentState == KeybindState.Released)
                {
                    Keybind bind = Keybinds.First((x) => x.KeybindName == kb.KeybindName);
                    bind.CurrentState = KeybindState.NotHeld;
                    kb.WasPressed = false;
                }

                if (kb.KeysPressed.Count > 0 && !kb.WasPressed)
                {
                    if (kb.CurrentState == KeybindState.NotHeld)
                    {
                        Keybind bind = Keybinds.First((x) => x.KeybindName == kb.KeybindName);
                        bind.CurrentState = KeybindState.Pressed;
                    }
                }

                if (kb.KeysPressed.Count == 0 && kb.WasPressed)
                {
                    if (kb.CurrentState == KeybindState.Held)
                    {
                        Keybind bind = Keybinds.First((x) => x.KeybindName == kb.KeybindName);
                        bind.CurrentState = KeybindState.Released;
                    }
                }
            }
        }

        public static Keybind GetKeybind(string KeybindName)
        {
            return Keybinds.First((x) => x.KeybindName == KeybindName);
        }

        public static GamePadState Controller(PlayerIndex playerNumber)
        {
            return GamePad.GetState(playerNumber);
        }
    }
    public enum KeybindState
    {
        Pressed,
        Held,
        Released,
        NotHeld
    }

    public class Keybind
    {
        public Keybind(string name, List<Keys> keys = null, List<Buttons> controllerButtons = null, PlayerIndex playerID = PlayerIndex.One)
        {
            KeybindName = name;
            KeybindKeys = keys;
            KeybindGamepadButtons = controllerButtons;
            PlayerID = playerID;
            CurrentState = KeybindState.NotHeld;
            Input.Keybinds.Add(this);
        }

        public string KeybindName;
        public List<Keys> KeybindKeys;
        public List<Buttons> KeybindGamepadButtons;
        public PlayerIndex PlayerID;
        internal bool WasPressed = false;
        internal List<bool> KeysPressed = new List<bool>();
        public KeybindState CurrentState;
    }
}
