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
            for (int y = 0; y < Sheet.Count; y++)
            {
                for (int x = 0; x < Sheet[y].Count; x++)
                {
                    ContentLoader.DisposeTexture(ImageName + "_" + y + ":" + x);
                }
            }

            Sheet = null;
            ImageName = "";
        }
    }
}
