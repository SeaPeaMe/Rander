using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Rander._2D
{
    class Button2DComponent : OffsetComponent2D
    {
        static bool OtherButtonIsPressed = false;
        public event Action OnClick;
        public event Action OnClickOutside;
        public event Action OnRelease;
        public event Action OnPress;
        public event Action OnHover;
        public event Action OnEnter;
        public event Action OnExit;

        bool IsBeingClicked = false;
        bool WasIn = false;
        bool WasClicked = false;

        List<Vector2> Corners = new List<Vector2>();
        List<float> CursorDistances = new List<float>();

        #region Creation
        public Button2DComponent(Action onClick = null, Action onClickOutside = null, Action onRelease = null, Action onPress = null, Action onHover = null, Action onEnter = null, Action onExit = null)
        {
            OnClick += onClick;
            OnClickOutside += onClickOutside;
            OnRelease += onRelease;
            OnPress += onPress;
            OnHover += onHover;
            OnEnter += onEnter;
            OnExit += onExit;

            OnClick += () =>
            {
                if (OtherButtonIsPressed == false)
                {
                    OtherButtonIsPressed = true;
                    IsBeingClicked = true;
                }
            };

            OnRelease += () =>
            {
                if (IsBeingClicked) {
                    OtherButtonIsPressed = false;
                    IsBeingClicked = false;
                }
            };
        }
        #endregion

        public override void Update()
        {
            if (!OtherButtonIsPressed || IsBeingClicked) {
                Corners.Clear();
                Corners.Add(LinkedObject.GetCorner(Alignment.TopLeft));
                Corners.Add(LinkedObject.GetCorner(Alignment.BottomLeft));
                Corners.Add(LinkedObject.GetCorner(Alignment.TopRight));
                Corners.Add(LinkedObject.GetCorner(Alignment.BottomRight));

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
                    }
                }
                else
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
                    }
                    else if (!WasClicked && Input.Mouse.LeftButton == ButtonState.Released)
                    {
                        WasClicked = false;
                    }
                }

                if (WasClicked && Input.Mouse.LeftButton == ButtonState.Released)
                {
                    WasClicked = false;
                    if (OnRelease != null) OnRelease();
                }

                if (Input.Mouse.LeftButton == ButtonState.Pressed && IsBeingClicked)
                {
                    if (OnPress != null) OnPress();
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

        public override void Draw()
        {
            if (Corners.Count != 0 && Debug.ShowButtonBounds == true && (!OtherButtonIsPressed || IsBeingClicked))
            {
                Game.Drawing.DrawLine(Corners[0], Corners[1], Color.Blue, 1);
                Game.Drawing.DrawLine(Corners[0], Corners[2], Color.Blue, 1);
                Game.Drawing.DrawLine(Corners[0], Corners[3], Color.Blue, 1);
                Game.Drawing.DrawLine(Corners[3], Corners[1], Color.Blue, 1);
                Game.Drawing.DrawLine(Corners[3], Corners[2], Color.Blue, 1);
            }
        }
    }
}
