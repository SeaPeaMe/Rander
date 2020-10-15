
/////////////////////////////////////
using Microsoft.Xna.Framework;
/// DEBUG MENU CAN BE OPENED WITH ///
///           LSHIFT + `          ///
/////////////////////////////////////

using Rander._2D;
using Rander.TestScripts;

namespace Rander
{
    public class DebugMenu : Component2D
    {

        bool DebugOpen = false;
        Object2D Parent;

        public override void Start()
        {
            WaitForInput();
        }

        void WaitForInput()
        {
            Time.WaitUntil(() => Input.Keys.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift) && Input.Keys.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.OemTilde), () =>
            {
                //Keys Pressed Down

                if (!DebugOpen)
                {
                    // Show Debug Menu
                    OpenMenu();
                }
                else
                {
                    Parent.Dispose(true);
                }

                DebugOpen = !DebugOpen;

                Time.WaitUntil(() => !Input.Keys.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift) && !Input.Keys.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.OemTilde), () =>
                {
                    // Keys Released
                    WaitForInput();
                });
            });
        }

        void OpenMenu()
        {
            Parent = new Object2D("DebugMenu", new Vector2(0, 0), new Vector2(0, 0), 0, new Component2D[] { new Spacer2DComponent(SpacerOption.VerticalSpacer, new Vector2(0, 5), Alignment.TopLeft, new Vector2(5, 5)), new Image2DComponent(DefaultValues.PixelTexture, Color.Black, 0) }, layer: 1);
            Parent.AddChild(new Object2D("DebugMenu_FPS", new Vector2(0, 0), new Vector2(120, 30), 0, new Component2D[] { new Text2DComponent("FPS", DefaultValues.DefaultFont, Color.Green, 0, 0.5f, textBreaking: false, subLayer: 0), new FPSScript() }), 0);
        }
    }
}
