using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Rander._2D;
using System.IO;

namespace Rander
{
    class Presets
    {
        public static Object2D Load(string path)
        {
            if (!File.Exists(path)) Debug.LogError("Failure loading preset \"" + Path.GetFileName(path) + "\", file doesn't exist!", true);
            Game.graphics.EndDraw();
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.Formatting = Formatting.None;
            settings.TypeNameHandling = TypeNameHandling.Auto;
            JsonSerializer Json = JsonSerializer.Create(settings);
            Object2D obj = Json.Deserialize<Object2D>(new JsonTextReader(new StringReader(File.ReadAllText(path))));

            obj.Position = new Vector2(float.PositiveInfinity, float.PositiveInfinity);

            obj.OnDeserialize();

            return obj;
        }

        public static void Save(Object2D instance, string path, bool Dispose = false)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.Formatting = Formatting.None;
            settings.TypeNameHandling = TypeNameHandling.Auto;
            JsonSerializer Json = JsonSerializer.Create(settings);

            if (!Directory.Exists(Path.GetDirectoryName(path))) Directory.CreateDirectory(Path.GetDirectoryName(path));

            TextWriter writer = File.CreateText(path);
            Json.Serialize(writer, instance);
            writer.Dispose();
            if (Dispose) instance.Dispose(true);
        }
    }
}
