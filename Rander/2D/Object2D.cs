using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Rander._2D
{
    public class Object2D
    {
        public string ObjectName;
        public Object2D Parent;
        public float Layer = 0;
        public bool Enabled = true;

        Alignment Al = Alignment.TopLeft;
        public Vector2 Pivot = Vector2.Zero;
        public event Action AlignmentChanged;
        #region Alignment/Pivot
        public Alignment Align { get { return Al; } set { SetPivot(value); } }

        public virtual void SetPivot(Alignment al)
        {
            Al = al;
            switch (al)
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

            if (AlignmentChanged != null) AlignmentChanged();
        }

        public virtual void SetPivot(Vector2 pivot)
        {
            Pivot = pivot;
        }
        #endregion

        float Rot = 0;
        float DestroyedParentRot = 0;
        public event Action RotationChanged;
        public event Action RelativeRotationChanged;
        #region Get/Set Rotation
        public float Rotation
        {
            get { return Rot; }
            set
            {
                foreach (Object2D Child in Children)
                {
                    Child.Rotation = value;
                    Child.RelativePosition = new Vector2((float)Math.Cos(MathHelper.ToRadians(Rotation)) * Vector2.Distance(Child.Pos, Pos), (float)Math.Sin(MathHelper.ToRadians(Rotation)) * Vector2.Distance(Child.Pos, Pos));
                }

                Rot = value;
                if (RotationChanged != null) RotationChanged();
                SetPivot(Align);
            }
        }
        public float RelativeRotation
        {
            get { if (Parent != null) { return Rot - Parent.Rot; } else { return Rot; } }
            set
            {
                if (Parent == null) // If the object has no parent, set the position normally
                {
                    Rotation = DestroyedParentRot + value;
                }
                else
                {
                    foreach (Object2D Child in Children)
                    {
                        Child.Rotation = value;
                        Child.RelativePosition = new Vector2((float)Math.Cos(MathHelper.ToRadians(Rotation)) * Child.RelativePosition.X, (float)Math.Sin(MathHelper.ToRadians(Rotation)) * Child.RelativePosition.Y);
                    }

                    SetPivot(Align);
                    Rot = value - Parent.Rot;
                }

                if (RelativePositionChanged != null) RelativeRotationChanged();
            }
        }
        #endregion

        Vector2 Pos = Vector2.Zero;
        Vector2 DestroyedParentPos = Vector2.Zero;
        public event Action PositionChanged;
        public event Action RelativePositionChanged;
        #region Get/Set Position
        public Vector2 RelativePosition
        {
            get { if (Parent != null) { return Pos - Parent.Position; } else { return Pos; } }
            set
            {
                // Sets the position and then updates all the children
                if (Parent == null) // If the object has no parent, set the position normally
                {
                    Position = DestroyedParentPos + value;
                }
                else
                {
                    Position = Parent.Position + value;
                }

                if (RelativePositionChanged != null) RelativePositionChanged();
            }
        }
        public Vector2 Position
        {
            get { return Pos; }
            set
            {
                foreach (Object2D Child in Children)
                {
                    Child.Position = value + Child.RelativePosition;
                }

                Pos = value;

                if (PositionChanged != null) PositionChanged();
            }
        }
        #endregion

        public List<Object2D> Children = new List<Object2D>();
        public delegate void Object2DEvent(Object2D object2d);
        public event Object2DEvent ChildAdded;
        #region Children
        public void AddChild(Object2D object2d)
        {
            Children.Add(object2d);
            object2d.Parent = this;
            if (ChildAdded != null) ChildAdded(object2d);
        }

        public void RemoveChild(Object2D object2d)
        {
            object2d.Parent = null;
            Children.Remove(object2d);
        }
        #endregion

        // Do relative size
        #region Get/Set Size
        public Vector2 Size = new Vector2(100, 100);
        #endregion

        #region Creation
        public Object2D(string objectName, Vector2 position, Vector2 size, float rotation = 0, Component2D[] components = null, Alignment alignment = Alignment.TopLeft, float layer = 0, Object2D parent = null)
        {
            Size = size;
            SetPivot(alignment);
            Position = position;
            Rotation = rotation;

            Layer = layer;

            ObjectName = objectName;

            Parent = parent;
            if (Parent != null)
            {
                Parent.AddChild(this);
            }

            // Goes through all the possible errors
            if (ObjectName == "")
            {
                Debug.LogError("Object name can't be blank!", true, 3);
            }
            else if (Level.Objects2D.ContainsKey(ObjectName))
            {
                Debug.LogError("The 2DObject \"" + ObjectName + "\" already exists!", true, 3);
            }
            else
            {
                Level.Objects2D.Add(ObjectName, this);
            }

            // Adds and Starts the scripts
            if (components != null)
            {
                foreach (Component2D com in components)
                {
                    AddComponent(com);
                }

                // Starts all the components after they've all been added
                foreach (Component2D com in Components)
                {
                    com.Start();
                }
            }
        }
        #endregion

        public List<Component2D> Components = new List<Component2D>();
        public delegate void Component2DEvent(Component2D component2d);
        public event Component2DEvent ComponentAdded;
        #region Components
        public Component2D AddComponent(Component2D component)
        {
            if (component != null)
            {
                Components.Add(component);
                component.LinkedObject = this;
            }

            if (ComponentAdded != null) ComponentAdded(component);

            return component;
        }

        public bool HasComponent<T>()
        {
            Component2D com = Components.Find(x => x is T);
            return com != null ? true : false;
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
        #endregion

        public virtual void Update()
        {
            if (Enabled)
            {
                foreach (Component2D Com in Components)
                {
                    Com.Update();
                }
            }
        }

        public virtual void Draw()
        {
            if (Enabled)
            {
                foreach (Component2D Com in Components)
                {
                    Com.Draw();
                }
            }
        }

        public void Dispose(bool DestroyChildren = false)
        {
            if (DestroyChildren == false)
            {
                // Re-binds the children to the "Being destroyed" object's parent
                foreach (Object2D Child in Children)
                {
                    Child.DestroyedParentPos = Pos;
                    Child.DestroyedParentRot = Rot;
                    Child.Parent = Parent;
                }
            }
            else
            {
                // Destroys the children
                foreach (Object2D Child in Children)
                {
                    Child.Dispose();
                }
            }

            // Runs OnDispose on all scripts
            foreach (Component2D Scr in Components)
            {
                Scr.OnDispose();
            }

            // Completely un-links the object
            Level.Objects2D.Remove(ObjectName);
        }
    }
}