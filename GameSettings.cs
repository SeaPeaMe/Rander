using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Rander
{
    public class GameSettings
    {
        public readonly static int TargetFPS = 60;
        public readonly static bool VSync = true;
        public readonly static SamplerState Filter = SamplerState.LinearClamp; // Texture mode. Use Linear point for pixelart
        public readonly static Color BackgroundColor = Color.CornflowerBlue;
        public readonly static Vector2 Resolution = Vector2.Zero; // Leave as Vector2.Zero for automatic resolution
        public readonly static bool FullScreen = true;

        public static void LoadSettings()
        {
            if (File.Exists(DefaultValues.ExecutableFolderPath + "/Settings.dat"))
            {
                JsonConvert.DeserializeObject<Screen>(File.ReadAllText(DefaultValues.ExecutableFolderPath + "/Settings.dat"));
            } else
            {
                JsonSerializerSettings set = new JsonSerializerSettings();
                set.Formatting = Formatting.None;
                set.ContractResolver = new StaticPropertyContractResolver();

                Screen.Resolution = Resolution;
                Screen.Fullscreen = FullScreen;
                Screen.TargetFPS = TargetFPS;
                Screen.VSync = VSync;
                Screen.BackgroundColor = BackgroundColor;
                Screen.Filter = Filter;

                Screen.ApplyChanges();

                string json = JsonConvert.SerializeObject(new Screen(), set);

                File.WriteAllText(DefaultValues.ExecutableFolderPath + "/Settings.dat", json);
            }
        }
    }

    public class StaticPropertyContractResolver : DefaultContractResolver
    {
        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            var baseMembers = base.GetSerializableMembers(objectType);

            PropertyInfo[] staticMembers = objectType.GetProperties(BindingFlags.Public | BindingFlags.Static);

            baseMembers.AddRange(staticMembers);

            return baseMembers;
        }
    }
}
