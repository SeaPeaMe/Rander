using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rander._2D
{
    class Dropdown2DComponent : Component2D
    {
        public List<Object2D> DropdownItems;
        public bool IsOpen = true;
        Object2D DropDownParent;

        bool MustClick = false;

        Vector2 Size;
        Color DropDownColor;
        Vector2 ItemSpacing;

        Dropdown2DComponent() { }

        public Dropdown2DComponent(Object2D[] dropdownItems, Vector2 size, Vector2 itemSpacing, Color dropDownColor, bool mustClick = false)
        {
            DropdownItems = dropdownItems.ToList();

            Size = size;
            MustClick = mustClick;
            DropDownColor = dropDownColor;
            ItemSpacing = itemSpacing;
        }

        public override void Start()
        {
            if (!LinkedObject.HasComponent<Button2DComponent>())
            {
                Debug.LogWarning("The 2DObject \"" + LinkedObject.ObjectName + "\" has a Dropdown 2D Component but no button to interact with it. A button 2D Component has automatically been added.", true);
                LinkedObject.AddComponent(new Button2DComponent());
            }

            DropDownParent = new Object2D("Dropdown_" + LinkedObject.ObjectName, LinkedObject.Position + new Vector2(0, LinkedObject.Size.Y / 2), Size, LinkedObject.Rotation, new Component2D[] { new Image2DComponent(DefaultValues.PixelTexture, DropDownColor), new Spacer2DComponent(SpacerOption.VerticalSpacer, ItemSpacing, Alignment.TopLeft, ItemSpacing) }, Alignment.TopLeft, LinkedObject.Layer, LinkedObject);

            if (MustClick) {
                LinkedObject.GetComponent<Button2DComponent>().OnClick += () => OpenDropdown();
                LinkedObject.GetComponent<Button2DComponent>().OnClickOutside += () => CloseDropdown();
            } else
            {
                LinkedObject.GetComponent<Button2DComponent>().OnEnter += () => OpenDropdown();
                LinkedObject.GetComponent<Button2DComponent>().OnExit += () => CloseDropdown();
            }

            foreach (Object2D obj in DropdownItems.ToArray())
            {
                DropDownParent.AddChild(obj);
            }

            CloseDropdown();
        }

        void CloseDropdown()
        {
            if (IsOpen)
            {
                foreach (Object2D obj in DropdownItems.ToArray())
                {
                    obj.Enabled = false;
                }

                DropDownParent.Enabled = false;

                IsOpen = false;
            }
        }

        void OpenDropdown()
        {
            if (!IsOpen)
            {
                DropDownParent.Enabled = true;

                foreach (Object2D obj in DropdownItems.ToArray())
                {
                    if (!DropDownParent.Children.Contains(obj)) {
                        DropDownParent.AddChild(obj);
                    } else
                    {
                        obj.Enabled = true;
                    }
                }

                IsOpen = true;
            }
        }
    }
}
