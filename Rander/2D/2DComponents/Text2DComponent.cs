using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Rander._2D
{
    public class Text2DComponent : Component2D
    {
        public string Text = "";
        public Color Color = Color.White;
        public float FontSize = 0.18f;
        SpriteFont Font = DefaultValues.DefaultFont;

        Alignment Al = Alignment.TopLeft;
        public Vector2 Pivot = Vector2.Zero;
        #region Alignment/Pivot
        public Alignment Align { get { return Al; } set { SetPivot(value); } }

        public void SetPivot(Alignment al)
        {
            Al = al;
            switch (al)
            {
                case Alignment.TopLeft:
                    Pivot = new Vector2(0, 0);
                    break;
                case Alignment.TopCenter:
                    Pivot = new Vector2((LinkedObject.Size.X - (Font.MeasureString(Text).X * FontSize)) / 2, 0);
                    break;
                case Alignment.TopRight:
                    Pivot = new Vector2(Font.MeasureString(Text).X, 0);
                    break;
                case Alignment.MiddleLeft:
                    Pivot = new Vector2(0, (LinkedObject.Size.Y - (Font.MeasureString(Text).Y * FontSize)) / 2);
                    break;
                case Alignment.Center:
                    Pivot = new Vector2((LinkedObject.Size.X - (Font.MeasureString(Text).X * FontSize)) / 2, (LinkedObject.Size.Y - (Font.MeasureString(Text).Y * FontSize)) / 2);
                    break;
                case Alignment.MiddleRight:
                    Pivot = new Vector2(Font.MeasureString(Text).X, (LinkedObject.Size.Y - (Font.MeasureString(Text).Y * FontSize)) / 2);
                    break;
                case Alignment.BottomLeft:
                    Pivot = new Vector2(0, Font.MeasureString(Text).Y);
                    break;
                case Alignment.BottomCenter:
                    Pivot = new Vector2((LinkedObject.Size.X - (Font.MeasureString(Text).X * FontSize)) / 2, Font.MeasureString(Text).Y);
                    break;
                case Alignment.BottomRight:
                    Pivot = new Vector2(Font.MeasureString(Text).X, Font.MeasureString(Text).Y);
                    break;
                default:
                    Pivot = new Vector2(0, 0);
                    break;
            }
        }
        #endregion

        #region Creation
        bool AutoSize = false;
        Alignment SetAl = Alignment.TopLeft;

        public Text2DComponent(string text)
        {
            Text = text;
            AutoSize = true;
        }

        public Text2DComponent(string text, SpriteFont font, Color color, float fontSize = 1, Alignment alignment = Alignment.TopLeft)
        {
            Text = text;
            FontSize = fontSize;
            Font = font;
            Color = color;
            SetAl = alignment;
        }

        public Text2DComponent(string text, SpriteFont font, Color color, Alignment alignment = Alignment.TopLeft)
        {
            Text = text;
            Font = font;
            Color = color;
            AutoSize = true;
            SetAl = alignment;
        }

        public override void Start()
        {
            if (AutoSize)
            {
                // Redo this, it's fucked
                FontSize = 0.10f;
            }

            SetPivot(SetAl);
        }
        #endregion

        public override void Draw()
        {
            Game.Drawing.DrawString(Font, Text, LinkedObject.PositionNoPivot + Pivot, Color, LinkedObject.Rotation, Vector2.Zero, FontSize, SpriteEffects.None, LinkedObject.Layer + 0.000000000001f);
        }
    }
}

public enum Alignment
{
    TopLeft = 0,
    TopCenter = 1,
    TopRight = 2,
    MiddleLeft = 3,
    Center = 4,
    MiddleRight = 5,
    BottomLeft = 6,
    BottomCenter = 7,
    BottomRight = 8
}
