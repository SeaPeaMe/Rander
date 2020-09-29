using Microsoft.Xna.Framework;
using Rander._2D;

namespace Rander.Examples
{
    class Test
    {

        public static void Start()
        {
            new Object2D("SliderTest", Screen.Resolution / 2, new Vector2(200, 30), 0, new Component2D[] { new Slider2DComponent() });
        }

        public static void Update()
        {
            
        }
    }
}
