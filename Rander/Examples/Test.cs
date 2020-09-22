using Microsoft.Xna.Framework;
using Rander._2D;

namespace Rander.Examples
{
    class Test
    {
        static Text2DComponent Txt;
        static string TextToDisplay = "Did the quick brown fox jump over the lazy dog?";

        public static void Start()
        {
            Txt = new Object2D("Txt", new Vector2(0, 0), new Vector2(1000, 30), 0, new Component2D[] { new Text2DComponent("", DefaultValues.DefaultFont, Color.White, 0, 1) }).GetComponent<Text2DComponent>();
            TypeText(Txt);
        }

        static int i = 0;
        static void TypeText(Text2DComponent txt)
        {
            if (i < TextToDisplay.Length) {
                txt.Text += TextToDisplay.Substring(i, 1);
                Time.Wait(10, () => TypeText(txt));
                i++;
            }
        }

        public static void Update()
        {
            
        }
    }
}
