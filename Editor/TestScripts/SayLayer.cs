using Rander._2D;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rander.Editor.TestScripts
{
    class SayLayer : Component2D
    {
        public override void Start()
        {
            Debug.Log(LinkedObject.ObjectName + "\nText:  " + (LinkedObject.HasComponent<Text2DComponent>() ? (LinkedObject.Layer + ((float)LinkedObject.GetComponent<Text2DComponent>().SubLayer / 10000)).ToString() : "null") + "\nImage: " + (LinkedObject.Layer + ((float)LinkedObject.GetComponent<Image2DComponent>().SubLayer / 10000)).ToString() + "\n");
        }
    }
}
