using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.Linq;

namespace Rander._2D
{
    public class Text2DComponent : OffsetComponent2D
    {
        string txt = "";
        public string Text { get { return txt; } set { txt = value; if (LinkedObject != null) { UpdateSize(); } } }
        public Color Color = Color.White;
        public float MaxFontSize = 0.18f;
        public float MinFontSize = 0.18f;
        public bool TextBreaking = true;
        public int SubLayer = 2;
        public string FontPath;
        SpriteFont Fnt = DefaultValues.DefaultFont;
        [JsonIgnore] public SpriteFont Font { get { return Fnt; } set { Fnt = value; FontPath = ContentLoader.LoadedFonts.First((x) => x.Value == Fnt).Key; } }

        Alignment Al = Alignment.TopLeft;
        public Vector2 Pivot = Vector2.Zero;
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

            PivotOffset = new Vector2(Font.MeasureString(Text).X * Pivot.X, Font.MeasureString(Text).Y * Pivot.Y);
        }
        #endregion

        #region Creation
        bool AutoSize = false;
        Alignment SetAl = Alignment.TopLeft;

        public Text2DComponent(string text, SpriteFont font, Color color, Rectangle offset, float fontSize = 1, Alignment alignment = Alignment.TopLeft, bool textBreaking = true, int subLayer = 1)
        {
            Text = text;
            MaxFontSize = fontSize;
            MinFontSize = fontSize;
            AutoSize = false;
            Font = font;
            Color = color;
            SetAl = alignment;
            SubLayer = subLayer;
            TextBreaking = textBreaking;
            Offset = offset;
        }

        public Text2DComponent(string text, SpriteFont font, Color color, float fontSize = 1, Alignment alignment = Alignment.TopLeft, bool textBreaking = true, int subLayer = 1)
        {
            Text = text;
            MaxFontSize = fontSize;
            MinFontSize = fontSize;
            AutoSize = false;
            Font = font;
            Color = color;
            SetAl = alignment;
            SubLayer = subLayer;
            TextBreaking = textBreaking;
        }

        public Text2DComponent(string text, SpriteFont font, Color color, float minFontSize = 0, float maxFontSize = 1, Alignment alignment = Alignment.TopLeft, bool textBreaking = true, int subLayer = 1)
        {
            Text = text;
            MaxFontSize = maxFontSize;
            MinFontSize = minFontSize;
            AutoSize = false;
            Font = font;
            Color = color;
            SetAl = alignment;
            SubLayer = subLayer;
            TextBreaking = textBreaking;
        }

        public Text2DComponent(string text, SpriteFont font, Color color, Rectangle offset, float minFontSize = 0, float maxFontSize = 1, Alignment alignment = Alignment.TopLeft, bool textBreaking = true, int subLayer = 1)
        {
            Text = text;
            MaxFontSize = maxFontSize;
            MinFontSize = minFontSize;
            AutoSize = false;
            Font = font;
            Color = color;
            SetAl = alignment;
            SubLayer = subLayer;
            TextBreaking = textBreaking;
            Offset = offset;
        }

        public override void OnDeserialize()
        {
            Font = FontPath == "" ? ContentLoader.LoadFont(FontPath) : DefaultValues.DefaultFont;
        }

        public override void Start()
        {
            UpdateSize();
        }
        #endregion

        /// <summary>
        /// The size of the text. Will be overwritten if text is changed
        /// </summary>
        public float FontSize;
        void UpdateSize()
        {
            if (LinkedObject.Size.X > 0 && LinkedObject.Size.Y > 0)
            {
                if (AutoSize)
                {
                    MinFontSize = LinkedObject.Size.Y / 100;
                    MeasureWidth:
                    if ((Font.MeasureString(Text) * MinFontSize).X > LinkedObject.Size.X)
                    {
                        MinFontSize -= 0.01f;
                        goto MeasureWidth;
                    }

                    MaxFontSize = MinFontSize;
                    FontSize = MinFontSize;
                }
                else
                {
                    // Calculates between min and max values
                    FontSize = MaxFontSize;
                    MeasureWidth:
                    if (FontSize > MinFontSize && (((Font.MeasureString(Text) * FontSize).X > LinkedObject.Size.X) || ((Font.MeasureString(Text) * FontSize).Y > LinkedObject.Size.Y)))
                    {
                        FontSize -= 0.01f;
                        goto MeasureWidth;
                    }

                    if (TextBreaking)
                    {
                        // Checks wether the text should break (But only if the text actually CAN go beyond the bounds)
                        if (FontSize <= MinFontSize && LinkedObject != null)
                        {
                            for (int i = 1; i <= Text.Length; i++)
                            {
                                if ((Font.MeasureString(Text.Substring(0, i)) * FontSize).X > LinkedObject.Size.X)
                                {
                                    Text = Text.Insert(i - 1, "\n");
                                }
                            }
                        }
                    }
                }
            }

            SetPivot(SetAl);
        }

        public override void Draw()
        {
            Game.Drawing.DrawString(Font, Text, LinkedObject.Position + Offset.Location.ToVector2(), Color, LinkedObject.MathRot, (LinkedObject.Size * LinkedObject.Pivot / FontSize) - (LinkedObject.Size * Pivot / FontSize) + PivotOffset, FontSize + Offset.Size.Y, SpriteEffects.None, LinkedObject.Layer + ((float)SubLayer / 1000));
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
