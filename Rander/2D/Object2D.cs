using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Rander._2D
{
    public class Object2D
    {
        public string ObjectName;
        Vector2 Pos;
        Vector2 ParentPiv = Vector2.Zero;
        public Vector2 Position { get { return Pos; } set {

                foreach (Object2D Child in Children)
                {
                    Child.Position = value + Child.RelativePosition;
                }

                Pos = value;
            }
        }
        public Vector2 Size;
        Alignment Al = Alignment.TopLeft;
        public Vector2 Pivot;
        public Alignment Align { get { return Al; } set { SetAlign(value); } }
        float Rot;
        float ParentRot;
        public float Rotation { get { return Rot; } set {
                foreach (Object2D Child in Children)
                {
                    Child.Rotation = value;
                    Child.RelativePosition = new Vector2((float)Math.Cos(Rotation) * Vector2.Distance(Child.Pos, Pos), (float)Math.Sin(Rotation) * Vector2.Distance(Child.Pos, Pos));
                }

                SetAlign(Align);
                Rot = value; 
            } 
        }
        public float RelativeRotation { get { if (Parent != null) { return Rot - Parent.Rot; } else { return Rot; } } set {

                if (Parent == null) // If the object has no parent, set the position normally
                {
                    Rotation = ParentRot + value;
                }
                else
                {
                    foreach (Object2D Child in Children)
                    {
                        Child.Rotation = value;
                        Child.RelativePosition = new Vector2((float)Math.Cos(Rotation) * Child.RelativePosition.X, (float)Math.Sin(Rotation) * Child.RelativePosition.Y);
                    }

                    SetAlign(Align);
                    Rot = value - Parent.Rot;
                }
            }
        }
        public List<Component2D> Components = new List<Component2D>();
        public float Layer = 0;

        public Vector2 RelativePosition { get { if (Parent != null) { return Pos - Parent.Pos; } else { return Pos; } } set {
                // Sets the position and then updates all the children
                if (Parent == null) // If the object has no parent, set the position normally
                {
                    Position = ParentPiv + value;
                }
                else
                {
                    Pos = Parent.Position + value;

                    foreach (Object2D Child in Children)
                    {
                        Child.Position = Pos + Child.RelativePosition;
                    }
                }
            }
        }
        public List<Object2D> Children = new List<Object2D>();
        public Object2D Parent;

        public Object2D(string objectName, Vector2 position, Vector2 size, float rotation = 0, Component2D[] components = null, Alignment alignment = Alignment.TopLeft, float layer = 0, Object2D parent = null)
        {
            Size = size;
            Pos = position;
            Rot = rotation;

            Layer = layer;

            ObjectName = objectName;

            Parent = parent;

            if (Parent != null) {
                Parent.AddChild(this);
            }

            SetAlign(alignment);

            // Goes through all the possible errors
            if (ObjectName == "")
            {
                Debug.LogError("Object name can't be blank!", true, 3);
            }
            else if (Game.Objects2D.ContainsKey(ObjectName))
            {
                Debug.LogError("The 2DObject \"" + ObjectName + "\" already exists!", true, 3);
            }
            else
            {
                Game.Objects2D.Add(ObjectName, this);
            }

            // Starts the scripts
            if (components != null)
            {
                foreach (Component2D com in components)
                {
                    AddComponent(com);
                }
            }
        }

        public virtual void Draw()
        {
            foreach (Component2D Com in Components)
            {
                Com.Draw();
            }
        }

        public virtual void SetAlign(Alignment al)
        {
            Al = al;
            switch (al)
            {
                case Alignment.TopLeft:
                    Pivot = new Vector2(0, 0);
                    break;
                case Alignment.TopCenter:
                    Pivot = new Vector2(Size.X / 2, 0);
                    break;
                case Alignment.TopRight:
                    Pivot = new Vector2(Size.X, 0);
                    break;
                case Alignment.MiddleLeft:
                    Pivot = new Vector2(0, Size.Y / 2);
                    break;
                case Alignment.Center:
                    Pivot = new Vector2(Size.X / 2, Size.Y / 2);
                    break;
                case Alignment.MiddleRight:
                    Pivot = new Vector2(Size.X, Size.Y / 2);
                    break;
                case Alignment.BottomLeft:
                    Pivot = new Vector2(0, Size.Y);
                    break;
                case Alignment.BottomCenter:
                    Pivot = new Vector2(Size.X / 2, Size.Y);
                    break;
                case Alignment.BottomRight:
                    Pivot = new Vector2(Size.X, Size.Y);
                    break;
                default:
                    Pivot = new Vector2(0, 0);
                    break;
            }
        }

        public Component2D AddComponent(Component2D component)
        {
            component.LinkedObject = this;
            Components.Add(component);
            component.Start();
            return component;
        }

        public T GetComponent<T>()
        {
            T Com = (T)Convert.ChangeType(Components.Find(x => x is T), typeof(T));

            if (Com != null)
            {
                return Com;
            }
            else
            {
                StackTrace stk = new StackTrace();
                Debug.LogError("2DObject \"" + ObjectName + "\" does not contain the component \"" + typeof(T).Name + "\"", true);
                return (T)Convert.ChangeType(null, typeof(T));
            }
        }

        public void RemoveComponent<T>()
        {
            Component2D Com = Components.Find(x => x is T);

            if (Com != null)
            {
                Components.Remove(Com);
            }
            else
            {
                Debug.LogError("2DObject \"" + ObjectName + "\" does not already contain the component \"" + typeof(T).Name + "\"");
            }
        }

        public void AddChild(Object2D object2d)
        {
            Children.Add(object2d);
        }

        public void Destroy(bool DestroyChildren = false)
        {
            if (DestroyChildren == false) {
                // Re-binds the children to the "Being destroyed" object's parent
                foreach (Object2D Child in Children)
                {
                    Child.ParentPiv = Pos;
                    Child.ParentRot = Rot;
                    Child.Parent = Parent;
                }
            } else
            {
                // Destroys the children
                foreach (Object2D Child in Children)
                {
                    Child.Destroy();
                }
            }

            // Completely un-links the object
            Game.Objects2D.Remove(ObjectName);
        }
    }
}