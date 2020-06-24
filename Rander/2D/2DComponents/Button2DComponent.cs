using Microsoft.Xna.Framework.Input;
using System;

namespace Rander._2D
{
    class Button2DComponent : Component2D
    {
        public Action OnClick = null; 
        public Action OnRelease = null; 
        public Action OnPress = null; 
        public Action OnHover = null; 
        public Action OnEnter = null; 
        public Action OnExit = null; 

        bool WasIn = false;
        bool WasClicked = false;

        #region Creation
        public Button2DComponent(Action onClick = null, Action onRelease = null, Action onPress = null, Action onHover = null, Action onEnter = null, Action onExit = null)
        {
            OnClick = onClick;
            OnRelease = onRelease;
            OnPress = onPress;
            OnHover = onHover;
            OnEnter = onEnter;
            OnExit = onExit;
        }
        #endregion

        public override void Update()
        {
            // When in the bounds of the rectangle
            if ((MouseInput.Position.X >= LinkedObject.PositionNoPivot.X) && (MouseInput.Position.X <= LinkedObject.PositionNoPivot.X + LinkedObject.Size.X) && (MouseInput.Position.Y >= LinkedObject.PositionNoPivot.Y - LinkedObject.Pivot.Y) && (MouseInput.Position.Y <= LinkedObject.PositionNoPivot.Y + LinkedObject.Size.Y))
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

                if (WasClicked && Input.Mouse.LeftButton == ButtonState.Released)
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
            }
        }
    }
}
