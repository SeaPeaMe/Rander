
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rander._2D._2DComponents
{
    class Input2DComponent : Component2D
    {
        SpriteFont Fnt;
        public SpriteFont Font { get { return Fnt; } set { Fnt = value; GhostText.Font = Fnt; InputTextComponent.Font = Fnt; } }
        Color GhstClr;
        public Color GhostTextColor { get { return GhstClr; } set { GhstClr = value; GhostText.Color = GhstClr; } }
        Color TxtClr;
        public Color InputTextColor { get { return TxtClr; } set { TxtClr = value; InputTextComponent.Color = TxtClr; } }

        public string InputText { get { return InputTextComponent.Text; } set { InputTextComponent.Text = value; } }

        Image2DComponent Caret;
        Text2DComponent InputTextComponent;
        Text2DComponent GhostText;
        Button2DComponent InputButton;
        Object2D InputSeperateObject;
        Object2D CaretObject;

        bool IsFocused = false;

        public Input2DComponent(string ghostText, SpriteFont font, Color ghostTextColor, Color inputTextColor, Color caretColor, float fontSize = 1, Alignment textAlignment = Alignment.MiddleLeft, Alignment alignment = Alignment.TopLeft)
        {
            // Text input handler
            Game.gameWindow.Window.TextInput += TextInput;

            InputTextComponent = new Text2DComponent("", font, inputTextColor, fontSize, textAlignment, 10);
            GhostText = new Text2DComponent(ghostText, font, ghostTextColor, fontSize, textAlignment, 11);
            InputButton = new Button2DComponent(new Action(OnClick), new Action(OnClickOutside));
            Caret = new Image2DComponent(DefaultValues.PixelTexture, caretColor, 12);

            Font = font;
            GhostTextColor = ghostTextColor;
            InputTextColor = inputTextColor;
        }

        public override void Start()
        {
            // Creates a whole new object JUST for the input to prevent confusion when looking for components in the main object this component is attatched to
            InputSeperateObject = new Object2D("Input_" + LinkedObject.ObjectName, LinkedObject.Position, LinkedObject.Size, 0, new Component2D[] { InputTextComponent, GhostText, InputButton }, LinkedObject.Align, LinkedObject.Layer, LinkedObject);
        }

        void TextInput(object sender, TextInputEventArgs args)
        {
            if (IsFocused) {
                if (!IllegalKeys.Contains(args.Key)) {
                    if (InputTextComponent.Font.Characters.Contains(args.Character)) {
                        InputText += args.Character;
                    }
                } else if (args.Key == Keys.Back && InputText != "")
                {
                    InputText = InputText.Substring(0, InputText.Length - 1);
                }

                UpdateCaret();
            }
        }

        void UpdateCaret()
        {
            CaretObject.RelativePosition = 
                  new Vector2(InputTextComponent.Font.MeasureString(InputText).X * InputTextComponent.FontSize, 0)
                + new Vector2(2, 0)
                + new Vector2(0, LinkedObject.Size.Y * InputTextComponent.Pivot.Y);
        }

        void OnClick()
        {
            GhostText.Color = Color.Transparent;

            float CaretSize = InputTextComponent.FontSize * 100;
            if (CaretObject == null) {
                CaretObject = new Object2D("CaretObj_" + LinkedObject.ObjectName, LinkedObject.Position - new Vector2((LinkedObject.Pivot.X * LinkedObject.Size.X), (CaretSize / 2) - (LinkedObject.Size.Y * InputTextComponent.Pivot.Y)), new Vector2(2, CaretSize), 0, new Component2D[] { Caret }, InputTextComponent.Align, LinkedObject.Layer, InputSeperateObject);
            }

            UpdateCaret();

            IsFocused = true;
        }

        void OnClickOutside()
        {
            IsFocused = false;

            if (CaretObject != null) {
                CaretObject.Destroy(true);
                CaretObject = null;
            }

            if (InputText == "")
            {
                GhostText.Color = GhostTextColor;
            }
        }

        #region Keys
        Keys[] IllegalKeys = new Keys[] {
            Keys.Back,
            Keys.Insert,
            Keys.Home,
            Keys.PageUp,
            Keys.PageDown,
            Keys.Delete,
            Keys.End,
            Keys.NumLock
        };
        #endregion

        //Keys[] PrevKeys = new Keys[] { };
        //List<Keys> CurrentKeys = new List<Keys>();
        //bool CapsLock = false;
        //public override void Update()
        //{
        //    if (IsFocused) {
        //        CurrentKeys = Input.Keys.GetPressedKeys().ToList();

        //        // I had to do this because an if statement comparing the two arrays wasn't working for some reason
        //        bool Caps = CurrentKeys.Contains(Keys.LeftShift) || CurrentKeys.Contains(Keys.RightShift);
        //        foreach (Keys key in CurrentKeys.ToList())
        //        {
        //            if (LegalChars.ContainsKey(key) && !PrevKeys.Contains(key))
        //            {
        //                KeyInfo inf;
        //                if (LegalChars.TryGetValue(key, out inf))
        //                {
        //                    InputText += Caps || CapsLock ? inf.UpperCase : inf.LowerCase;
        //                }
        //            } else if (!PrevKeys.Contains(key)) // Special Keys
        //            {
        //                if (key == Keys.Back && InputText != "")
        //                {
        //                    InputText = InputText.Substring(0, InputText.Length - 1);
        //                }
        //            }
        //        }

        //        PrevKeys = CurrentKeys.ToArray();
        //    }
        //}
    }
}
