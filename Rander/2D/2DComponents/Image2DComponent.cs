using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rander._2D
{
    public class Image2DComponent : Component2D
    {
        public Texture2D Texture;
        public Color Color = Color.White;


        public Image2DComponent(Texture2D texture)
        {
            Texture = texture;
        }

        public Image2DComponent(Texture2D texture, Color color)
        {
            Texture = texture;
            Color = color;
        }

        public override void Draw()
        {
            Game.Drawing.Draw(Texture, new Rectangle((int)LinkedObject.Position.X, (int)LinkedObject.Position.Y, (int)LinkedObject.Size.X, (int)LinkedObject.Size.Y), new Rectangle((int)LinkedObject.Position.X, (int)LinkedObject.Position.Y, (int)LinkedObject.Size.X, (int)LinkedObject.Size.Y), Color, LinkedObject.Rotation, LinkedObject.Pivot, SpriteEffects.None, LinkedObject.Layer);
        }
    }
}
