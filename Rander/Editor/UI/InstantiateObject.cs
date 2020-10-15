using Microsoft.Xna.Framework;
using Rander._2D;

namespace Rander.Editor
{
    class InstantiateObject : Component2D
    {
        public void Ins()
        {
            Object2D Obj = Presets.Load(LinkedObject.ObjectName);
            Obj.Position = new Vector2(Rand.RandomFloat(0, Screen.Resolution.X), Rand.RandomFloat(0, Screen.Resolution.Y));
            UI.AddWindow.Dispose(true);
        }
    }
}
