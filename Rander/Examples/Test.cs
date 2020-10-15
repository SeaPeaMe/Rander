using Microsoft.Xna.Framework;
using Rander._2D;

namespace Rander.Examples
{
    class Test
    {
        static SpriteSheet sht;
        public static void Start()
        {
            sht = ContentLoader.LoadSpriteSheet("Editor/TestImages/Floors1.png", new Vector2(16, 16));
            new Object2D("SliderTest", Screen.Resolution / 2, new Vector2(50, 50), 0, new Component2D[] { new Image2DComponent(sht.Sheet[0][1]) });
        }

        public static void Update()
        {

        }
    }
}
