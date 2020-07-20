using Rander;
using Rander._2D;

namespace ExampleGame.Scripts
{
    class PlanetMove : Component2D
    {
        public override void Update()
        {
            LinkedObject.Position += MenuStarMove.StarMoveSpeed * Time.FrameTime;
        }
    }
}
