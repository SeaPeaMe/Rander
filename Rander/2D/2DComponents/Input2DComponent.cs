
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
        public SpriteFont Font { get { return Fnt; } set { Fnt = value; GhostTextComponent.Font = Fnt; InputTextComponent.Font = Fnt; } }
        Color GhstClr;
        public Color GhostTextColor { get { return GhstClr; } set { GhstClr = value; GhostTextComponent.Color = GhstClr; } }
        Color TxtClr;
        public Color InputTextColor { get { return TxtClr; } set { TxtClr = value; InputTextComponent.Color = TxtClr; } }

        public string InputText { get { return InputTextComponent.Text; } set { InputTextComponent.Text = value; } }

        public string GhostText { get { return GhostTextComponent.Text; } set { GhostTextComponent.Text = value; } }

        public int CaretBlinkSpeed = 1000;

        Image2DComponent Caret;
        Text2DComponent InputTextComponent;
        Text2DComponent GhostTextComponent;
        Button2DComponent InputButton;
        Object2D InputSeperateObject;
        Object2D CaretObject;

        bool IsFocused = false;

        public Input2DComponent(string ghostText, SpriteFont font, Color ghostTextColor, Color inputTextColor, Color caretColor, float minFontSize = 0, float maxFontSize = 1, Alignment textAlignment = Alignment.MiddleLeft, Alignment alignment = Alignment.TopLeft)
        {
            // Text input handler
            Game.gameWindow.Window.TextInput += TextInput;

            InputTextComponent = new Text2DComponent(" ", font, inputTextColor, minFontSize, maxFontSize, textAlignment, 10);
            GhostTextComponent = new Text2DComponent(ghostText + " ", font, ghostTextColor, minFontSize, maxFontSize, textAlignment, 11);
            InputButton = new Button2DComponent(new Action(OnClick), new Action(OnClickOutside));
            Caret = new Image2DComponent(DefaultValues.PixelTexture, caretColor, 12);

            Font = font;
            GhostTextColor = ghostTextColor;
            InputTextColor = inputTextColor;
        }

        public override void Start()
        {
            // Creates a whole new object JUST for the input to prevent confusion when looking for components in the main object this component is attatched to
            InputSeperateObject = new Object2D("Input_" + LinkedObject.ObjectName, LinkedObject.Position, LinkedObject.Size, 0, new Component2D[] { InputTextComponent, GhostTextComponent, InputButton }, LinkedObject.Align, LinkedObject.Layer, LinkedObject);

            // Sets the input text to nothing once it's been instantiated, so it can be aligned correctly
            InputText = "";
            GhostText = GhostText.Substring(0, GhostText.Length - 1);
        }

        System.Timers.Timer CaretBlinkTimer;
        void CaretBlink()
        {
            if (Caret.Color.A == 255)
            {
                Caret.Color.A = 0;
            } else if (Caret.Color.A == 0)
            {
                Caret.Color.A = 255;
            }

            CaretBlinkTimer = Time.Wait(CaretBlinkSpeed, new Action(CaretBlink));
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
                + new Vector2(LinkedObject.Size.X * (InputTextComponent.Pivot.X - 0.5f), LinkedObject.Size.Y * (InputTextComponent.Pivot.Y - 0.5f));
        }

        void OnClick()
        {
            GhostTextComponent.Color = Color.Transparent;

            if (CaretBlinkTimer != null)
            {
                CaretBlinkTimer.Stop();
                CaretBlinkTimer.Dispose();
                CaretBlinkTimer = null;
            }

            float CaretSize = InputTextComponent.FontSize * 100;
            if (CaretObject == null) {
                CaretObject = new Object2D("CaretObj_" + LinkedObject.ObjectName, LinkedObject.Position - new Vector2((LinkedObject.Pivot.X * LinkedObject.Size.X), (CaretSize / 2) - (LinkedObject.Size.Y * InputTextComponent.Pivot.Y)), new Vector2(2, CaretSize), 0, new Component2D[] { Caret }, InputTextComponent.Align, LinkedObject.Layer, InputSeperateObject);
            }

            UpdateCaret();

            CaretBlink();

            IsFocused = true;
        }

        void OnClickOutside()
        {
            IsFocused = false;

            if (InputText == "")
            {
                GhostTextComponent.Color = GhostTextColor;
            }

            if (CaretBlinkTimer != null)
            {
                CaretBlinkTimer.Stop();
                CaretBlinkTimer.Dispose();
                CaretBlinkTimer = null;
            }

            if (CaretObject != null)
            {
                CaretObject.Destroy(true);
                CaretObject = null;
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
    }
}
