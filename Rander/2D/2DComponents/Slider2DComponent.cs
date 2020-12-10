using Microsoft.Xna.Framework;
using System;

namespace Rander._2D
{
    public class Slider2DComponent : Component2D
    {
        public float MaxValue { get { return max; } set { max = value; if (!IsUpdating) UpdateSlider(); } }
        float max;
        public float MinValue { get { return min; } set { min = value; if (!IsUpdating) UpdateSlider(); } }
        float min;
        public float Value { get { return val; } set { val = value; if (!IsUpdating) UpdateSlider(); } }
        float val;

        public Orientation Orientation;
        public Image2DComponent Handle;
        public Image2DComponent Bar;
        public Image2DComponent Fill;
        public bool Enabled = true;
        public bool AllowDecimals = false;
        Button2DComponent HandleButton;
        Object2D Sl;

        public Slider2DComponent(float min, float max, Orientation orientation, bool allowDecimals = false)
        {
            // Prevents update slider methodfrom runnins before the parent is allocated
            IsUpdating = true;

            MinValue = min;
            MaxValue = max;
            Orientation = orientation;
            AllowDecimals = allowDecimals;

            Bar = new Image2DComponent(DefaultValues.PixelTexture, Color.DarkGray, Alignment.TopLeft, 0);
            Fill = new Image2DComponent(DefaultValues.PixelTexture, Color.Gray, Alignment.TopLeft, 1);
            Handle = new Image2DComponent(DefaultValues.PixelTexture, Color.White, Alignment.TopCenter, 2);
            HandleButton = new Button2DComponent(onPress: () => MoveHandle());
            IsUpdating = false;

        }

        public override void Start()
        {
            Sl = new Object2D("Slider_" + LinkedObject.ObjectName, Vector2.Zero, LinkedObject.Size, LinkedObject.Rotation, new Component2D[] { Bar, Fill, Handle, HandleButton }, LinkedObject.Align, LinkedObject.Layer, LinkedObject);

            UpdateSlider();
        }

        public override void Update()
        {
            if (Orientation == Orientation.Horizontal)
            {
                LinkedObject.Rotation = 0;
            } else
            {
                LinkedObject.Rotation = -90;
            }
        }

        float NormalisedValue;
        void MoveHandle()
        {
            if (Enabled) {
                if (Orientation == Orientation.Horizontal)
                {
                    NormalisedValue = (MouseInput.Position.X - (int)LinkedObject.GetCorner(Alignment.MiddleLeft).X) / LinkedObject.Size.X;
                }
                else
                {
                    NormalisedValue = ((int)LinkedObject.GetCorner(Alignment.MiddleLeft).Y - MouseInput.Position.Y) / LinkedObject.Size.X;
                }

                Value = Math.Clamp((NormalisedValue * (MaxValue + Math.Abs(MinValue))) + MinValue, MinValue, MaxValue);
                UpdateSlider();
            }
        }

        bool IsUpdating = false;
        void UpdateSlider()
        {
            IsUpdating = true;
            Value = AllowDecimals ? Value : (int)Math.Round(Value);

            int ValueOffsetFromLeft = (int)(LinkedObject.Size.X / (MaxValue + Math.Abs(MinValue)) * (Value + Math.Abs(MinValue)));

            if (Orientation == Orientation.Horizontal) {
                Handle.Offset = new Rectangle((int)Math.Clamp(ValueOffsetFromLeft, (int)LinkedObject.Size.Y / 2, LinkedObject.Size.X - (int)LinkedObject.Size.Y / 2), 0, -(int)LinkedObject.Size.X + (int)LinkedObject.Size.Y, -(int)LinkedObject.Size.Y + (int)LinkedObject.Size.Y);
            } else
            {
                Handle.Offset = new Rectangle(0, (int)-Math.Clamp(ValueOffsetFromLeft, (int)LinkedObject.Size.Y / 2, LinkedObject.Size.X - (int)LinkedObject.Size.Y / 2), -(int)LinkedObject.Size.X + (int)LinkedObject.Size.Y, -(int)LinkedObject.Size.Y + (int)LinkedObject.Size.Y);
            }

            Fill.Offset = new Rectangle(0, 0, (int)Math.Clamp(ValueOffsetFromLeft - LinkedObject.Size.X, -LinkedObject.Size.X, LinkedObject.Size.X), 0);
            Fill.RenderRegion = new Rectangle(0, 0, (int)(Fill.Texture.Width / (MaxValue + Math.Abs(MinValue)) * Value), Fill.Texture.Height);
            IsUpdating = false;
        }

        public override void OnDispose()
        {
            Sl.Dispose(true);
        }
    }

    public enum Orientation
    {
        Horizontal,
        Vertical
    }
}
