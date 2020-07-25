using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Rander._2D
{
    class Button2DComponent : Component2D
    {
        public Action OnClick = null;
        public Action OnClickOutside = null;
        public Action OnRelease = null; 
        public Action OnPress = null; 
        public Action OnHover = null; 
        public Action OnEnter = null; 
        public Action OnExit = null; 

        bool WasIn = false;
        bool WasClicked = false;

        Vector2 Left;
        Vector2 Top;
        Vector2 Bottom;
        Vector2 Right;
        List<Vector2> Corners = new List<Vector2>();
        List<float> CursorDistances = new List<float>();

        #region Creation
        public Button2DComponent(Action onClick = null, Action onClickOutside = null, Action onRelease = null, Action onPress = null, Action onHover = null, Action onEnter = null, Action onExit = null)
        {
            OnClick = onClick;
            OnClickOutside = onClickOutside;
            OnRelease = onRelease;
            OnPress = onPress;
            OnHover = onHover;
            OnEnter = onEnter;
            OnExit = onExit;
        }
        #endregion

        public override void Update()
        {

            // Calculates side points
            float RotOffset = -((LinkedObject.Pivot.X - 0.5f) * 180);
            Vector2 MagnitudeTopBottom = new Vector2(LinkedObject.Size.X * Math.Abs(LinkedObject.Pivot.X - 0.5f));
            Vector2 PosOffsetTop = new Vector2(-(float)Math.Sin(MathHelper.ToRadians(LinkedObject.Rotation)), (float)Math.Cos(MathHelper.ToRadians(LinkedObject.Rotation))) * LinkedObject.Size.Y * LinkedObject.Pivot.Y;
            Vector2 PosOffsetBottom = new Vector2(-(float)Math.Sin(MathHelper.ToRadians(LinkedObject.Rotation)), (float)Math.Cos(MathHelper.ToRadians(LinkedObject.Rotation))) * LinkedObject.Size.Y * -(1 - LinkedObject.Pivot.Y);

            Vector2 MagnitudeRight = new Vector2(LinkedObject.Size.X * Math.Abs(1 - LinkedObject.Pivot.X));
            Vector2 MagnitudeLeft = new Vector2(LinkedObject.Size.X * Math.Abs(LinkedObject.Pivot.X));
            Vector2 PosOffsetLeftRight = new Vector2(-(float)Math.Sin(MathHelper.ToRadians(LinkedObject.Rotation)), (float)Math.Cos(MathHelper.ToRadians(LinkedObject.Rotation))) * LinkedObject.Size.Y * (LinkedObject.Pivot.Y - 0.5f);

            Top = LinkedObject.Position - new Vector2(-(float)Math.Sin(MathHelper.ToRadians(LinkedObject.Rotation + RotOffset)), (float)Math.Cos(MathHelper.ToRadians(LinkedObject.Rotation + RotOffset))) * MagnitudeTopBottom - PosOffsetTop;
            Bottom = LinkedObject.Position - new Vector2(-(float)Math.Sin(MathHelper.ToRadians(LinkedObject.Rotation + RotOffset)), (float)Math.Cos(MathHelper.ToRadians(LinkedObject.Rotation + RotOffset))) * MagnitudeTopBottom - PosOffsetBottom;
            Left = LinkedObject.Position - new Vector2((float)Math.Cos(MathHelper.ToRadians(LinkedObject.Rotation)), (float)Math.Sin(MathHelper.ToRadians(LinkedObject.Rotation))) * MagnitudeLeft - PosOffsetLeftRight;
            Right = LinkedObject.Position + new Vector2((float)Math.Cos(MathHelper.ToRadians(LinkedObject.Rotation)), (float)Math.Sin(MathHelper.ToRadians(LinkedObject.Rotation))) * MagnitudeRight - PosOffsetLeftRight;

            // Calculates the corners of the button to triangulate the rectangle
            Corners.Clear();
            Vector2 CornerOffset = new Vector2(-(float)Math.Sin(MathHelper.ToRadians(LinkedObject.Rotation)), (float)Math.Cos(MathHelper.ToRadians(LinkedObject.Rotation))) * LinkedObject.Size.Y * MathHelper.Clamp(LinkedObject.Pivot.Y + 0.5f, 0, 0.5f);
            Corners.Add(Left - CornerOffset); // Top Left
            Corners.Add(Left + CornerOffset); // Bottom Left
            Corners.Add(Right - CornerOffset); // Top Right
            Corners.Add(Right + CornerOffset); // Bottom Right

            // Calculates distances to each corner
            CursorDistances.Clear();
            CursorDistances.Add(Vector2.Distance(MouseInput.Position.ToVector2(), Corners[0]));
            CursorDistances.Add(Vector2.Distance(MouseInput.Position.ToVector2(), Corners[1]));
            CursorDistances.Add(Vector2.Distance(MouseInput.Position.ToVector2(), Corners[2]));
            CursorDistances.Add(Vector2.Distance(MouseInput.Position.ToVector2(), Corners[3]));

            // Calculates triangle sizes
            float RotRectArea = CalcTriangleArea(CursorDistances[0], CursorDistances[1], LinkedObject.Size.Y) + CalcTriangleArea(CursorDistances[1], CursorDistances[3], LinkedObject.Size.X) + CalcTriangleArea(CursorDistances[3], CursorDistances[2], LinkedObject.Size.Y) + CalcTriangleArea(CursorDistances[2], CursorDistances[0], LinkedObject.Size.X);

            // When in the bounds of the rectangle
            if (LinkedObject.Size.X * LinkedObject.Size.Y + 1f >= RotRectArea)
            {
                if (OnHover != null) OnHover();

                if (!WasIn)
                {
                    WasIn = true;
                    if (OnEnter != null) OnEnter();
                }

                if (Input.Mouse.LeftButton == ButtonState.Pressed && WasClicked == false)
                {
                    WasClicked = true;
                    if (OnClick != null) OnClick();
                } else if (WasClicked && Input.Mouse.LeftButton == ButtonState.Released)
                {
                    WasClicked = false;
                    if (OnRelease != null) OnRelease();
                }

                if (Input.Mouse.LeftButton == ButtonState.Pressed)
                {
                    if (OnPress != null) OnPress();
                }
            } else
            {
                if (WasIn)
                {
                    WasIn = false;
                    if (OnExit != null) OnExit();
                }

                if (Input.Mouse.LeftButton == ButtonState.Pressed && WasClicked == false)
                {
                    WasClicked = true;
                    if (OnClickOutside != null) OnClickOutside();
                } else if (WasClicked && Input.Mouse.LeftButton == ButtonState.Released)
                {
                    WasClicked = false;
                }
            }
        }

        float CalcTriangleArea(float d1, float d2, float d3)
        {
            // Uses Heron's Formula to find triangle's area using 3 sides
            // https://en.wikipedia.org/wiki/Heron's_formula

            float s = (d1 + d2 + d3) / 2;
            return (float)Math.Sqrt(s * (s - d1) * (s - d2) * (s - d3));
        }

        // This was for debugging the bounds of the button
        // -----------------------------------------------
        //public override void Draw()
        //{
        //    Game.Drawing.Draw(DefaultValues.PixelTexture, Left, null, Color.Blue, 0, Vector2.Zero, 10, SpriteEffects.None, 1);
        //    Game.Drawing.Draw(DefaultValues.PixelTexture, Right, null, Color.Yellow, 0, Vector2.Zero, 10, SpriteEffects.None, 1);
        //    Game.Drawing.Draw(DefaultValues.PixelTexture, Top, null, Color.Red, 0, Vector2.Zero, 10, SpriteEffects.None, 1);
        //    Game.Drawing.Draw(DefaultValues.PixelTexture, Bottom, null, Color.Green, 0, Vector2.Zero, 10, SpriteEffects.None, 1);

        //    Game.Drawing.Draw(DefaultValues.PixelTexture, Corners[0], null, Color.Coral, 0, Vector2.Zero, 15, SpriteEffects.None, 1);
        //    Game.Drawing.Draw(DefaultValues.PixelTexture, Corners[1], null, Color.Coral, 0, Vector2.Zero, 10, SpriteEffects.None, 1);
        //    Game.Drawing.Draw(DefaultValues.PixelTexture, Corners[2], null, Color.Coral, 0, Vector2.Zero, 10, SpriteEffects.None, 1);
        //    Game.Drawing.Draw(DefaultValues.PixelTexture, Corners[3], null, Color.Coral, 0, Vector2.Zero, 10, SpriteEffects.None, 1);
        //}
    }
}
