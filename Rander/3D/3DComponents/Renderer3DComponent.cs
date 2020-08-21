using Microsoft.Xna.Framework.Graphics;

namespace Rander._3D._3DComponents
{
    class Renderer3DComponent : Component3D
    {
        Model ObjectModel;

        public Renderer3DComponent(Model model)
        {
            ObjectModel = model;
        }

        public override void Draw()
        {
            ObjectModel.Draw(LinkedObject.WorldMatrix, Level.ActiveCamera.ViewMatrix, Level.ActiveCamera.ProjectionMatrix);
        }
    }
}
