using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace Rander._2D
{
    public class Text2DComponent : Component2D
    {
        public string Text = "";
        public Color Color = Color.White;
        public float FontSize = 0.18f;
        SpriteFont Font = DefaultValues.DefaultFont;

        Alignment Al = Alignment.TopLeft;
        Vector2 Pivot = Vector2.Zero;
        Vector2 PivotOffset = Vector2.Zero;
        #region Alignment/Pivot
        public Alignment Align { get { return Al; } set { SetPivot(value); } }

        public virtual void SetPivot(Alignment al)
        {
            Al = al;
            switch (al)
            {
                case Alignment.TopLeft:
                    Pivot = new Vector2(0, 0);
                    break;
                case Alignment.TopCenter:
                    Pivot = new Vector2(0.5f, 0);
                    break;
                case Alignment.TopRight:
                    Pivot = new Vector2(1, 0);
                    break;
                case Alignment.MiddleLeft:
                    Pivot = new Vector2(0, 0.5f);
                    break;
                case Alignment.Center:
                    Pivot = new Vector2(0.5f, 0.5f);
                    break;
                case Alignment.MiddleRight:
                    Pivot = new Vector2(1, 0.5f);
                    break;
                case Alignment.BottomLeft:
                    Pivot = new Vector2(0, 1);
                    break;
                case Alignment.BottomCenter:
                    Pivot = new Vector2(0.5f, 1);
                    break;
                case Alignment.BottomRight:
                    Pivot = new Vector2(1, 1);
                    break;
                default:
                    Pivot = new Vector2(0, 0);
                    break;
            }

            PivotOffset = new Vector2(Font.MeasureString(Text).X * Pivot.X, Font.MeasureString(Text).Y * Pivot.Y) + new Vector2(0, -5);
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
            Game.Drawing.DrawString(Font, Text, LinkedObject.Position, Color, MathHelper.ToRadians(LinkedObject.Rotation), (LinkedObject.Size * LinkedObject.Pivot / FontSize) - (LinkedObject.Size * Pivot / FontSize) + PivotOffset, FontSize, SpriteEffects.None, LinkedObject.Layer + 0.000000000001f);
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
