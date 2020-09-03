using Microsoft.Xna.Framework;

namespace Rander._2D
{
    class Spacer2DComponent : Component2D
    {

        public Vector2 Offset;
        public SpacerOption SpacerOption;
        public Vector2 Spacing;
        public Alignment ChildAlign;
        Vector2 Pivot;

        Spacer2DComponent() { }

        public Spacer2DComponent(SpacerOption spacerOption, Vector2 spacing, Alignment childAlign, Vector2 offset)
        {
            Offset = offset;
            SpacerOption = spacerOption;
            Spacing = spacing;
            ChildAlign = childAlign;

            switch (ChildAlign)
            {
                case Alignment.TopLeft:
                    Pivot = new Vector2(0, 0);
                    break;
                case Alignment.TopCenter:
                    Pivot = new Vector2(0.5f, 0);
                    break;
                case Alignment.TopRight:
                    Pivot = new Vector2(1, 0);
                    break;
                case Alignment.MiddleLeft:
                    Pivot = new Vector2(0, 0.5f);
                    break;
                case Alignment.Center:
                    Pivot = new Vector2(0.5f, 0.5f);
                    break;
                case Alignment.MiddleRight:
                    Pivot = new Vector2(1, 0.5f);
                    break;
                case Alignment.BottomLeft:
                    Pivot = new Vector2(0, 1);
                    break;
                case Alignment.BottomCenter:
                    Pivot = new Vector2(0.5f, 1);
                    break;
                case Alignment.BottomRight:
                    Pivot = new Vector2(1, 1);
                    break;
                default:
                    Pivot = new Vector2(0, 0);
                    break;
            }
        }

        public override void Start()
        {
            LinkedObject.ChildAdded += SortChildren;
        }

        void SortChildren(Object2D object2d)
        {
            int i = 0;
            Vector2 ChildOffset = Vector2.Zero;
            ChildOffset -= Offset * 2;
            if (SpacerOption == SpacerOption.VerticalSpacer)
            {
                // Sets child offset for centering
                foreach (Object2D Child in LinkedObject.Children)
                {
                    Child.SetPivot(ChildAlign);
                    ChildOffset += new Vector2(0, (Child.Size.Y + Spacing.Y) * Child.Pivot.Y);
                    i++;
                }
                ChildOffset /= 2;

                i = 0;
                foreach (Object2D child in LinkedObject.Children)
                {
                    child.RelativePosition = Vector2.Zero;
                    child.RelativePosition += (LinkedObject.Size * Pivot) - (LinkedObject.Size * LinkedObject.Pivot) - ChildOffset; // Initial Spacing 
                    ChildOffset -= new Vector2(0, Spacing.Y + child.Size.Y);
                    i++;
                }
            }
            else if (SpacerOption == SpacerOption.HorizontalSpacer)
            {
                // Sets child offset for centering
                foreach (Object2D Child in LinkedObject.Children)
                {
                    Child.SetPivot(ChildAlign);
                    ChildOffset += new Vector2((Child.Size.X + Spacing.X) * Child.Pivot.X, 0);
                    i++;
                }
                ChildOffset /= 2;

                i = 0;
                foreach (Object2D child in LinkedObject.Children)
                {
                    child.RelativePosition = Vector2.Zero;
                    child.RelativePosition += (LinkedObject.Size * Pivot) - (LinkedObject.Size * LinkedObject.Pivot) - ChildOffset; // Initial Spacing
                    ChildOffset -= new Vector2(Spacing.X + child.Size.X, 0);
                    i++;
                }
            }
        }

        public override void OnDispose()
        {
            LinkedObject.ChildAdded -= SortChildren;
        }
    }

    enum SpacerOption
    {
        VerticalSpacer,
        HorizontalSpacer
    }
}
