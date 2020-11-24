using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.Linq;

namespace Rander._2D
{
    public class Image2DComponent : OffsetComponent2D
    {
        internal string TexturePath;
        Texture2D Tex = DefaultValues.PixelTexture;
        [JsonIgnore] public Texture2D Texture { get { return Tex; } set { Tex = value; if (ContentLoader.Loaded2DTextures.ContainsValue(Tex)) { TexturePath = ContentLoader.Loaded2DTextures.First((x) => x.Value == Tex).Key; } else { Debug.LogError("The texture being assigned has not been loaded properly!", true, 3); } RenderRegion = value.Bounds; } }
        public Color Color = Color.White;
        public int SubLayer = 0;
        public Rectangle RenderRegion;

        #region Pivoting
        Alignment Al;
        public Alignment Align { get { return Al; } set { SetPivot(value); } }
        Vector2 Pivot;

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
        }
        #endregion

        #region Creation
        Image2DComponent() { }

        public Image2DComponent(Texture2D texture, int subLayer = 0)
        {
            Texture = texture;
            SubLayer = subLayer;
            RenderRegion = texture.Bounds;
            Time.WaitUntil(() => LinkedObject != null, () => Align = LinkedObject.Align);
        }

        public Image2DComponent(Texture2D texture, Color color, int subLayer = 0)
        {
            Texture = texture;
            Color = color;
            SubLayer = subLayer;
            RenderRegion = texture.Bounds;
            Time.WaitUntil(() => LinkedObject != null, () => Align = LinkedObject.Align);
        }

        public Image2DComponent(Texture2D texture, Color color, Alignment alignment, int subLayer = 0)
        {
            Texture = texture;
            Color = color;
            SubLayer = subLayer;
            RenderRegion = texture.Bounds;
            Align = alignment;
        }

        public Image2DComponent(Texture2D texture, Rectangle offset, Color color, int subLayer = 0)
        {
            Texture = texture;
            Color = color;
            SubLayer = subLayer;
            RenderRegion = texture.Bounds;
            Offset = offset;
            Time.WaitUntil(() => LinkedObject != null, () => Align = LinkedObject.Align);
        }

        public Image2DComponent(Texture2D texture, Rectangle offset, Alignment alignment, Color color, int subLayer = 0)
        {
            Texture = texture;
            Color = color;
            SubLayer = subLayer;
            RenderRegion = texture.Bounds;
            Offset = offset;
            Align = alignment;
        }

        public Image2DComponent(Texture2D texture, Rectangle offset, Rectangle renderRegion, Color color, int subLayer = 0)
        {
            Texture = texture;
            Color = color;
            SubLayer = subLayer;
            RenderRegion = renderRegion;
            Offset = offset;
            Time.WaitUntil(() => LinkedObject != null, () => Align = LinkedObject.Align);
        }

        public Image2DComponent(Texture2D texture, Rectangle offset, Alignment alignment, Rectangle renderRegion, Color color, int subLayer = 0)
        {
            Texture = texture;
            Color = color;
            SubLayer = subLayer;
            RenderRegion = renderRegion;
            Offset = offset;
            Align = alignment;

        }
        #endregion

        public override void OnDeserialize()
        {
            Texture = TexturePath == "" ? DefaultValues.PixelTexture : ContentLoader.LoadTexture(TexturePath);
        }

        public override void Draw()
        {
            Rectangle Rect = new Rectangle(LinkedObject.Position.ToPoint() + Offset.Location, LinkedObject.Size.ToPoint() + Offset.Size);
            Game.Drawing.Draw(Texture, Rect, RenderRegion, Color, MathHelper.ToRadians(LinkedObject.Rotation), Pivot * new Vector2(Texture.Width, Texture.Height), SpriteEffects.None, LinkedObject.Layer + ((float)SubLayer / 1000));
        }
    }
}
