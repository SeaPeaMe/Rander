using Microsoft.Xna.Framework;
using Rander._2D;

namespace Rander.Examples
{
    public class Test : BaseScript
    {
        Object2D Lvl;
        Object2D Chl;
        public override void Start()
        {
            float TileSize = 50;
            float Gap = 5;

            Lvl = new Object2D("Level", Vector2.Zero, new Vector2(TileSize), 0);
            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < 20; x++)
                {
                    Chl = Lvl.AddChild(new Object2D("BunchTest_" + x + "_" + y, new Vector2((x * TileSize) + x * Gap, (y * TileSize) + y * Gap), new Vector2(TileSize), 0, new Component2D[] { new Image2DComponent(DefaultValues.PixelTexture, Color.White) }));
                }
            }
        }

        public override void FixedUpdate()
        {
            Lvl.Size += Vector2.One * 0.1f;
            Lvl.Rotation += 0.01f;
            Debug.Log(Chl.Position.ToString());
        }
    }
}
