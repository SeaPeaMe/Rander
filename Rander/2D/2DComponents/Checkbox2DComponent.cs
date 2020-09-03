using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Rander._2D
{
    class Checkbox2DComponent : Component2D
    {
        public Action OnClick = null;
        public Action OnClickOutside = null;
        public Action OnRelease = null;
        public Action OnPress = null;
        public Action OnHover = null;
        public Action OnEnter = null;
        public Action OnExit = null;

        public bool IsDown = false;
        public Texture2D UpTexture = DefaultValues.PixelTexture;
        public Texture2D DownTexture = DefaultValues.PixelTexture;
        public Color UpColor;
        public Color DownColor;

        Button2DComponent Button;
        Image2DComponent Image;
        Object2D ChkObj;
        #region Creation
        Checkbox2DComponent() { }

        public Checkbox2DComponent(Color upColor, Color downColor, Texture2D upTexture = null, Texture2D downTexture = null)
        {
            UpColor = upColor;
            DownColor = downColor;

            UpTexture = upTexture != null ? upTexture : DefaultValues.PixelTexture;
            DownTexture = downTexture != null ? downTexture : DefaultValues.PixelTexture;
        }

        public Checkbox2DComponent(Texture2D upTexture, Color upColor, Texture2D downTexture, Color downColor, Action onClick = null, Action onClickOutside = null, Action onRelease = null, Action onPress = null, Action onHover = null, Action onEnter = null, Action onExit = null)
        {
            // The base keyword will call the default button creation thing
            UpColor = upColor;
            DownColor = downColor;

            UpTexture = upTexture != null ? upTexture : DefaultValues.PixelTexture;
            DownTexture = downTexture != null ? downTexture : DefaultValues.PixelTexture;

            OnClick = onClick;
            OnClickOutside = onClickOutside;
            OnRelease = onRelease;
            OnPress = onPress;
            OnHover = onHover;
            OnEnter = onEnter;
            OnExit = onExit;
        }

        public override void Start()
        {
            Button = new Button2DComponent(OnClick, OnClickOutside, new Action(OnCheckClick), OnPress, OnHover, OnEnter, OnExit);
            Image = new Image2DComponent(IsDown ? DownTexture : UpTexture, IsDown ? DownColor : UpColor);
            ChkObj = new Object2D("Check_" + LinkedObject.ObjectName, LinkedObject.Position, LinkedObject.Size, LinkedObject.Rotation, new Component2D[] { Image, Button }, LinkedObject.Align, LinkedObject.Layer, LinkedObject);
        }
        #endregion

        void OnCheckClick()
        {
            IsDown = !IsDown;
            Image.Texture = IsDown ? DownTexture : UpTexture;
            Image.Color = IsDown ? DownColor : UpColor;

            if (OnRelease != null) OnRelease();
        }

        public override void OnDispose()
        {
            ChkObj.Dispose(true);
        }
    }
}
