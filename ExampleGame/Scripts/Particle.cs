using Microsoft.Xna.Framework;
using Rander;
using Rander._2D;

namespace ExampleGame.Scripts
{
    class Particle : Component2D
    {
        int ID;

        public Particle(int ParticleID)
        {
            ID = ParticleID;
        }

        public override void Start()
        {
            LinkedObject.Position = MouseInput.Position.ToVector2();

            if (ID == int.MaxValue - 1)
            {
                Rander.Game.FindObject2D("Cursor").GetComponent<CursorReplacement>().i = 0;
            }
        }
    }
}
