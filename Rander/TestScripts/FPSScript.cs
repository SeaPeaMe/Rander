using Rander._2D;

namespace Rander.TestScripts
{
    class FPSScript : Component2D
    {

        float TimeToPass;
        int FramesPassed = 0;

        public override void Draw()
        {
            if ((float)Game.Gametime.TotalGameTime.TotalSeconds > TimeToPass)
            {
                LinkedObject.GetComponent<Text2DComponent>().Text = "FPS: " + FramesPassed;
                TimeToPass += 1;
                FramesPassed = 0;
            }
            else
            {
                FramesPassed += 1;
            }
        }
    }
}
