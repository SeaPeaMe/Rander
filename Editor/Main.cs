using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rander._2D;
using Microsoft.Xna.Framework;

namespace Rander.Editor
{
    class Main
    {
        public static void Init()
        {
            Debug.LogSuccess("----- BEGIN EDITOR -----");
            Debug.LogWarning("Init...");
            Game.BackgroundColor = Color.CornflowerBlue;
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
            Properties = new Object2D("Editor_PropertiesContainer", new Vector2(Screen.Width, 0), new Vector2(300, Screen.Height), 0, new Component2D[] { new Image2DComponent(DefaultValues.PixelTexture, EditorTheme.Background, 0) }, Alignment.TopRight, EditorUILayer);
        }
    }

    class EditorTheme
    {
        public static Color Background = new Color(60, 60, 60);
    }
}
