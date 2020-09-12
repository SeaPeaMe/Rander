using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.Linq;

namespace Rander._2D
{
    public class Image2DComponent : Component2D
    {
        public string TexturePath;
        Texture2D Tex = DefaultValues.PixelTexture;
        [JsonIgnore] public Texture2D Texture { get { return Tex; } set { Tex = value; TexturePath = ContentLoader.Loaded2DTextures.First((x) => x.Value == Tex).Key; } }
        public Color Color = Color.White;
        public int SubLayer = 0;

        #region Creation
        Image2DComponent() { }

        public Image2DComponent(Texture2D texture, int subLayer = 0)
        {
            Texture = texture;
            SubLayer = subLayer;
        }

        public Image2DComponent(Texture2D texture, Color color, int subLayer = 0)
        {
            Texture = texture;
            Color = color;
            SubLayer = subLayer;
        }
        #endregion

        public override void OnDeserialize()
        {
            Texture = TexturePath == "" ? DefaultValues.PixelTexture : ContentLoader.LoadTexture(TexturePath);
        }

        public override void Draw()
        {
            Rectangle Rect = new Rectangle(LinkedObject.Position.ToPoint(), LinkedObject.Size.ToPoint());
            Game.Drawing.Draw(Texture, Rect, null, Color, MathHelper.ToRadians(LinkedObject.Rotation), LinkedObject.Pivot * new Vector2(Texture.Width, Texture.Height), SpriteEffects.None, LinkedObject.Layer + ((float)SubLayer / 100000));
        }
    }
}
