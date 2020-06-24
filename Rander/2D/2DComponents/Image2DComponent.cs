using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rander._2D
{
    public class Image2DComponent : Component2D
    {
        public Texture2D Texture = DefaultValues.PixelTexture;
        public Color Color = Color.White;

        #region Creation
        public Image2DComponent(Texture2D texture)
        {
            Texture = texture;
        }

        public Image2DComponent(Texture2D texture, Color color)
        {
            Texture = texture;
            Color = color;
        }
        #endregion

        public override void Draw()
        {
            Game.Drawing.Draw(Texture, LinkedObject.PositionNoPivot, null, Color, LinkedObject.Rotation, new Vector2(0, 0), LinkedObject.Size, SpriteEffects.None, LinkedObject.Layer);
        }
    }
}
