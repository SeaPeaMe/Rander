using Microsoft.Xna.Framework;
using Rander._2D;

namespace Rander.Editor
{
    class InstantiateObject : Component2D
    {
        public void Ins()
        {
            foreach (Object2D item in Level.Objects2D.Values)
            {
                Debug.Log(item.ObjectName);
            }

            Object2D obj = Serialization.Load<Object2D>(LinkedObject.ObjectName);
            UI.AddWindow.Dispose(true);

            Debug.Log("\n" + obj.ObjectName + ", " + obj.HasComponent<Image2DComponent>() + "\n");
            obj.Position = Screen.Resolution / 2;

            foreach (var item in obj.Components)
            {
                Debug.Log(item.GetType().Name);
            }
        }
    }
}
