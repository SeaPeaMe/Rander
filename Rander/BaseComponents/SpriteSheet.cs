using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Rander
{
    public class SpriteSheet
    {
        public List<List<Texture2D>> Sheet = new List<List<Texture2D>>();
        public Vector2 SheetSize;
        internal string ImageName;

        public void Dispose()
        {
            int y = 0;
            foreach (var Row in Sheet)
            {
                int x = 0;
                foreach (var item in Row)
                {
                    ContentLoader.DisposeTexture(ImageName + "_" + y + ":" + x);
                    x++;
                }
                y++;
            }

            Sheet.Clear();
            ImageName = "";
        }
    }
}
