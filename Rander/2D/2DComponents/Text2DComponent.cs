using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rander._2D
{
    public class Text2DComponent : Component2D
    {
        public string Txt = "";
        public Color Color = Color.White;
        public float FontSize { get { return FontRect.X; } set { FontRect = new Vector2(value, value); } }
        public Vector2 FontRect;
        SpriteFont Font = DefaultValues.DefaultFont;

        Alignment Al;
        public Vector2 Pivot;
        public Alignment Align { get { return Al; } set { SetAlign(value); } }

        public Text2DComponent(string text)
        {
            Txt = text;
        }

        public Text2DComponent(string text, SpriteFont font, Color color, float fontSize = 1, Alignment alignment = Alignment.TopLeft)
        {
            Txt = text;
            FontSize = fontSize;
            Font = font;
            Color = color;
            SetAlign(alignment);
        }

        public Text2DComponent(string text, SpriteFont font, Color color, Alignment alignment = Alignment.TopLeft)
        {
            Txt = text;
            Font = font;
            Color = color;
            FontRect = LinkedObject.Size;
            SetAlign(alignment);
        }

        public void SetAlign(Alignment al)
        {
            Al = al;
            switch (al)
            {
                case Alignment.TopLeft:
                    Pivot = new Vector2(0, 0);
                    break;
                case Alignment.TopCenter:
                    Pivot = new Vector2(Font.MeasureString(Txt).X / 2, 0);
                    break;
                case Alignment.TopRight:
                    Pivot = new Vector2(Font.MeasureString(Txt).X, 0);
                    break;
                case Alignment.MiddleLeft:
                    Pivot = new Vector2(0, Font.MeasureString(Txt).Y / 2);
                    break;
                case Alignment.Center:
                    Pivot = new Vector2(Font.MeasureString(Txt).X / 2, Font.MeasureString(Txt).Y / 2);
                    break;
                case Alignment.MiddleRight:
                    Pivot = new Vector2(Font.MeasureString(Txt).X, Font.MeasureString(Txt).Y / 2);
                    break;
                case Alignment.BottomLeft:
                    Pivot = new Vector2(0, Font.MeasureString(Txt).Y);
                    break;
                case Alignment.BottomCenter:
                    Pivot = new Vector2(Font.MeasureString(Txt).X / 2, Font.MeasureString(Txt).Y);
                    break;
                case Alignment.BottomRight:
                    Pivot = new Vector2(Font.MeasureString(Txt).X, Font.MeasureString(Txt).Y);
                    break;
                default:
                    Pivot = new Vector2(0, 0);
                    break;
            }
        }

        public override void Draw()
        {
            Game.Drawing.DrawString(Font, Txt, LinkedObject.Position, Color, LinkedObject.Rotation, Pivot, FontRect, SpriteEffects.None, LinkedObject.Layer);
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
