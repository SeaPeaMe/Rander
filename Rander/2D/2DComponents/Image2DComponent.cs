using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rander._2D
{
    public class Image2DComponent : Component2D
    {
        public Texture2D Texture = DefaultValues.PixelTexture;
        public Color Color = Color.White;
        public int SubLayer = 0;

        #region Creation
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

        public override void Draw()
        {
            Rectangle Rect = new Rectangle(LinkedObject.Position.ToPoint(), LinkedObject.Size.ToPoint());
            Game.Drawing.Draw(Texture, Rect, null, Color, MathHelper.ToRadians(LinkedObject.Rotation), LinkedObject.Pivot * new Vector2(Texture.Width, Texture.Height), SpriteEffects.None, LinkedObject.Layer + (SubLayer / 1000000000));
        }
    }
}
