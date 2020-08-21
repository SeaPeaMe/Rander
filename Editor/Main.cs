using Microsoft.Xna.Framework;
using Rander._2D;

namespace Rander.Editor
{
    class Main
    {
        public static void Init()
        {
            Debug.LogSuccess("----- BEGIN EDITOR -----");
            Debug.LogWarning("Init...");
            Game.BackgroundColor = new Color(40, 40, 40);

            UI.Init();
        }
    }

    class UI
    {
        const int EditorUILayer = 1;
        static Object2D Properties;

        public static void Init()
        {
            Debug.Log("    UI");
            // Properties Bar
            Properties = new Object2D("Editor_PropertiesContainer", new Vector2(Screen.Resolution.X, Screen.Resolution.Y), new Vector2(300, Screen.Resolution.Y - 50), 0, new Component2D[] {
                new Image2DComponent(DefaultValues.PixelTexture, EditorTheme.Window, 0),
                new Spacer2DComponent(SpacerOption.VerticalSpacer, new Vector2(0, 10), Alignment.TopCenter, new Vector2(0, 10))
            }, Alignment.BottomRight, EditorUILayer - 0.00000000000005f);

            new Object2D("Editor_PropertiesContainerTitle", Properties.Position + new Vector2(0, -Properties.Size.Y), new Vector2(300, 50), 0, new Component2D[] {
                new Image2DComponent(DefaultValues.PixelTexture, EditorTheme.TitleBar),
                new Text2DComponent("Properties", DefaultValues.DefaultFont, EditorTheme.Window, 0, 0.2f, Alignment.Center, textBreaking: false)
            }, Alignment.BottomRight, EditorUILayer - 0.00000000000005f);
        }
    }

    class EditorTheme
    {
        public static Color Window = new Color(60, 60, 60);
        public static Color TitleBar = new Color(120, 120, 120);
    }
}
