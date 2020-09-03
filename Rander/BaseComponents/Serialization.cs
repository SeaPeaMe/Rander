using System;
using System.Collections.Generic;
using System.Text;

using System.Xml.Serialization;
using System.IO;
using Rander._2D;
using System.Windows.Navigation;

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace Rander
{
    class Serialization
    {
        public static T Load<T>(string path)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.Formatting = Formatting.None;
            settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
            JsonSerializer Json = JsonSerializer.Create(settings);

            object obj = Json.Deserialize<T>(new JsonTextReader(new StringReader(File.ReadAllText(path))));

            if (obj.GetType() == typeof(Object2D)) (obj as Object2D).OnDeserialize();


            return (T)obj;
        }

        public static void Save(object instance, string path)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.Formatting = Formatting.None;
            JsonSerializer Json = JsonSerializer.Create(settings);
            TextWriter writer = File.CreateText(path);
            Json.Serialize(writer, instance);
            writer.Dispose();
        }
    }
}
