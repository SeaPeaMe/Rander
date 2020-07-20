using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using Rander._2D;

namespace Rander
{
    public class Level
    {
        public static Dictionary<SoundEffectInstance, SoundEffect> Sounds = new Dictionary<SoundEffectInstance, SoundEffect>();
        public static Dictionary<string, Object2D> Objects2D = new Dictionary<string, Object2D>();

        public static void ClearLevel()
        {
            Game.PauseGame = true;
            Debug.LogWarning("--- CLEARING LEVEL ---");
            Debug.LogWarning("Disposing Objects & Instances...");
            Debug.Log("     2D Objects...");
            Objects2D.Clear();
            Debug.Log("     Sound Instances...");
            for (int i = 0; i < Sounds.Count; i++)
            {
                Sounds.Keys.ToArray()[i].Stop();
                Sounds.Keys.ToArray()[i].Dispose();
            }
            Sounds.Clear();

            Debug.LogWarning("Disposing Timers...");
            for (int i = 0; i < Game.Timers.Count;)
            {
                Game.Timers[0].Stop();
                Game.Timers[0].Dispose();
                Game.Timers.RemoveAt(0);
            }
            Debug.LogSuccess("--- LEVEL CLEAR SUCCESS ---");
            Game.PauseGame = false;
        }

        #region Object2D
        public static Object2D FindObject2D(string objectName)
        {
            Object2D obj;
            if (Objects2D.TryGetValue(objectName, out obj))
            {
                return obj;
            }
            else
            {
                Debug.LogError("Object \"" + objectName + "\" does not exist!");
                return null;
            }
        }

        public static bool Object2DExists(string objectName)
        {
            return Objects2D.Keys.Contains(objectName);
        }
        #endregion

        public static void Update()
        {
            // Update 2D Objects
            foreach (Object2D Obj in Objects2D.Values.ToList())
            {
                Obj.Update();
            }
        }

        public static void Draw()
        {
            // Draws 2D Objects
            foreach (Object2D Obj in Objects2D.Values.ToList())
            {
                Obj.Draw();
            }
        }
    }
}
