namespace Rander._2D
{
    public class Mask2DComponent : Component2D
    {

        public Image2DComponent Image;

        public override void Start()
        {
            Level.Masks.Add(this);
        }

        public void DrawMask()
        {
            Image.Draw();
        }

        public override void Update()
        {
            foreach (Object2D Obj in LinkedObject.Children.ToArray())
            {
                if (Obj.Position.X > LinkedObject.Position.X + LinkedObject.Size.X || Obj.Position.X < LinkedObject.Position.X || Obj.Position.Y < LinkedObject.Position.Y || Obj.Position.Y > LinkedObject.Position.Y + LinkedObject.Size.Y)
                {
                    Obj.Enabled = false;
                }
                else
                {
                    Obj.Enabled = true;
                }
            }
        }
    }
}
