using Microsoft.Xna.Framework.Audio;
using Rander._2D;
using Rander._3D;
using System.Collections.Generic;
using System.Linq;

namespace Rander
{
    public class Level
    {
        public static Dictionary<SoundEffectInstance, SoundEffect> Sounds = new Dictionary<SoundEffectInstance, SoundEffect>();
        public static Dictionary<string, Object2D> Objects2D = new Dictionary<string, Object2D>();
        public static Dictionary<string, Object3D> Objects3D = new Dictionary<string, Object3D>();
        public static List<Mask2DComponent> Masks = new List<Mask2DComponent>();

        public static Camera3DComponent Active3DCamera = null;

        public static void SaveLevel(string Path)
        {
            // TODO
        }

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
            Time.Timers.Clear();

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
                return null;
            }
        }

        public static bool Object2DExists(string objectName)
        {
            return Objects2D.Keys.Contains(objectName);
        }
        #endregion

        #region Object3D
        public static Object3D FindObject3D(string objectName)
        {
            Object3D obj;
            if (Objects3D.TryGetValue(objectName, out obj))
            {
                return obj;
            }
            else
            {
                Debug.LogError("Object3D \"" + objectName + "\" does not exist!");
                return null;
            }
        }

        public static bool Object3DExists(string objectName)
        {
            return Objects3D.Keys.Contains(objectName);
        }
        #endregion

        internal static void Update()
        {
            // Update 2D Objects
            foreach (Object2D Obj in Objects2D.Values.ToList())
            {
                Obj.Update();
            }

            // Update 3D Objects
            foreach (Object3D Obj in Objects3D.Values.ToList())
            {
                Obj.Update();
            }
        }

        internal static void FixedUpdate()
        {
            // Update 2D Objects
            foreach (Object2D Obj in Objects2D.Values.ToList())
            {
                Obj.FixedUpdate();
            }

            // Update 3D Objects
            foreach (Object3D Obj in Objects3D.Values.ToList())
            {
                Obj.FixedUpdate();
            }
        }

        internal static void Draw()
        {
            // Draws 2D Objects
            foreach (Object2D Obj in Objects2D.Values.ToList())
            {
                Obj.Draw();
            }

            // Draws 3D Objects
            foreach (Object3D Obj in Objects3D.Values.ToList())
            {
                Obj.Draw();
            }
        }
    }
}
