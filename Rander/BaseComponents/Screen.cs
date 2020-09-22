using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rander
{
    public class Screen
    {
        [JsonRequired]public static Vector2 Resolution;
        [JsonIgnore] public readonly static Vector2 DeviceResolution = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
        [JsonRequired] public static bool Fullscreen;
        [JsonRequired] public static bool AllowResizing = false;
        [JsonRequired] public static Color BackgroundColor;
        [JsonRequired] public static int TargetFPS = 60;
        [JsonRequired] public static bool VSync;
        [JsonRequired] public static SamplerState Filter;

        public Screen()
        {
            ApplyChanges();
        }

        public static void ApplyChanges()
        {
            if (Resolution == Vector2.Zero)
            {
                Game.graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                Game.graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }
            else
            {
                Game.graphics.PreferredBackBufferWidth = Resolution.ToPoint().X;
                Game.graphics.PreferredBackBufferHeight = Resolution.ToPoint().Y;
            }

            Game.graphics.IsFullScreen = Fullscreen;
            Game.gameWindow.Window.AllowUserResizing = AllowResizing;
            Game.graphics.SynchronizeWithVerticalRetrace = VSync;
            Game.gameWindow.TargetElapsedTime = TimeSpan.FromSeconds((float)1 / TargetFPS);
            Game.graphics.ApplyChanges();
        }
    }
}
