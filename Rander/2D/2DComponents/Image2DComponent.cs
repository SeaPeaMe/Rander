using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.Linq;

namespace Rander._2D
{
    public class Image2DComponent : DrawableComponent2D
    {
        internal string TexturePath;
        Texture2D Tex = DefaultValues.PixelTexture;
        [JsonIgnore] public Texture2D Texture { get { return Tex; } set { Tex = value; TexturePath = ContentLoader.Loaded2DTextures.First((x) => x.Value == Tex).Key; RenderRegion = value.Bounds; } }
        public Color Color = Color.White;
        public int SubLayer = 0;
        public Rectangle RenderRegion;

        #region Creation
        Image2DComponent() { }

        public Image2DComponent(Texture2D texture, int subLayer = 0)
        {
            Texture = texture;
            SubLayer = subLayer;
            RenderRegion = texture.Bounds;
        }

        public Image2DComponent(Texture2D texture, Color color, int subLayer = 0)
        {
            Texture = texture;
            Color = color;
            SubLayer = subLayer;
            RenderRegion = texture.Bounds;
        }

        public Image2DComponent(Texture2D texture, Rectangle offset, Color color, int subLayer = 0)
        {
            Texture = texture;
            Color = color;
            SubLayer = subLayer;
            RenderRegion = texture.Bounds;
            Offset = offset;
        }

        public Image2DComponent(Texture2D texture, Rectangle offset, Rectangle renderRegion, Color color, int subLayer = 0)
        {
            Texture = texture;
            Color = color;
            SubLayer = subLayer;
            RenderRegion = renderRegion;
            Offset = offset;
        }
        #endregion

        public override void OnDeserialize()
        {
            Texture = TexturePath == "" ? DefaultValues.PixelTexture : ContentLoader.LoadTexture(TexturePath);
        }

        public override void Draw()
        {
            Rectangle Rect = new Rectangle(LinkedObject.Position.ToPoint() + Offset.Location, LinkedObject.Size.ToPoint() + Offset.Size);
            Game.Drawing.Draw(Texture, Rect, RenderRegion, Color, MathHelper.ToRadians(LinkedObject.Rotation), LinkedObject.Pivot * new Vector2(Texture.Width, Texture.Height), SpriteEffects.None, LinkedObject.Layer + ((float)SubLayer / 100000));
        }
    }
}
