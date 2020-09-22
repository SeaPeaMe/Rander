using Rander._2D;

namespace Rander.Editor
{
    class LevelContent : Component2D
    {

        int LevelCount = 0;

        public override void Update()
        {
            if (LevelCount != Level.Objects2D.Count)
            {
                LevelCount = Level.Objects2D.Count;
            }
        }
    }
}
