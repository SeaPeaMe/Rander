using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rander._2D
{
    public class Object2D
    {
        public string ObjectName;
        public Object2D Parent;
        public float Layer = 0;
        bool En = true;
        public bool Enabled { get { return En; } set { foreach (Object2D Child in Children.ToArray()) { Child.Enabled = value; } En = value; } }

        Alignment Al = Alignment.TopLeft;
        public Vector2 Pivot = Vector2.Zero;
        public event Action AlignmentChanged;
        #region Alignment/Pivot
        public Alignment Align { get { return Al; } set { SetPivot(value); } }

        public Vector2 GetCorner(Alignment Corner)
        {
            Vector2 MagnitudeRight = new Vector2(Size.X * Math.Abs(1 - Pivot.X));
            Vector2 MagnitudeLeft = new Vector2(Size.X * Math.Abs(Pivot.X));
            Vector2 CornerOffset = new Vector2(-(float)Math.Sin(MathHelper.ToRadians(Rotation)), (float)Math.Cos(MathHelper.ToRadians(Rotation))) * Size.Y * (Pivot.Y - 0.5f);
            Vector2 Top = new Vector2((float)Math.Sin(MathHelper.ToRadians(Rotation)), -(float)Math.Cos(MathHelper.ToRadians(Rotation))) * Size.Y * MathHelper.Clamp(Pivot.Y + 0.5f, 0, 0.5f);

            Vector2 Circ = new Vector2((float)Math.Cos(MathHelper.ToRadians(Rotation)), (float)Math.Sin(MathHelper.ToRadians(Rotation)));
            Vector2 Left = -Circ * MagnitudeLeft - CornerOffset;
            Vector2 Right = Circ * MagnitudeRight - CornerOffset;
            
            Vector2 Center = Position + new Vector2((Right.X + Left.X) * MathHelper.Clamp(Pivot.Y + 0.5f, 0, 0.5f), (Right.Y + Left.Y) * MathHelper.Clamp(Pivot.X + 0.5f, 0, 0.5f));

            Left += Position;
            Right += Position;

            switch (Corner)
            {
                case Alignment.TopLeft:
                    return Left + Top;
                case Alignment.TopCenter:
                    return Center + Top;
                case Alignment.TopRight:
                    return Right + Top;
                case Alignment.MiddleLeft:
                    return Left;
                case Alignment.Center:
                    return Center;
                case Alignment.MiddleRight:
                    return Right;
                case Alignment.BottomLeft:
                    return Left - Top;
                case Alignment.BottomCenter:
                    return Center - Top;
                case Alignment.BottomRight:
                    return Right - Top;
                default:
                    return Vector2.Zero;
            }
        }

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

                    float Magnitude = Vector2.Distance(Pos, Child.Pos);
                    float RotationOffset = MathF.Atan(Size.Y / Size.X) + MathHelper.ToRadians(1);
                    Child.Position = Pos + new Vector2((float)Math.Cos(MathHelper.ToRadians(Rotation) + RotationOffset) * Magnitude, (float)Math.Sin(MathHelper.ToRadians(Rotation) + RotationOffset) * Magnitude);
                }

                Rot = value;
                if (RotationChanged != null) RotationChanged();
                SetPivot(Pivot);
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

                    SetPivot(Pivot);
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
        /// <summary>
        /// Adds a child and changes its layer to the parent's
        /// </summary>
        /// <param name="object2d">The parent 2D Object</param>
        /// <param name="SubLayer">The sub-layer of the child to the parent</param>
        public void AddChild(Object2D object2d, int SubLayer)
        {
            Children.Add(object2d);
            object2d.Parent = this;
            object2d.Layer = Layer + (SubLayer / 1000);
            if (ChildAdded != null) ChildAdded(object2d);
        }
        /// <summary>
        /// Adds a child, but does not change its Layer
        /// </summary>
        /// <param name="object2d">The parent 2D Object</param>
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
        Vector2 Sz = Vector2.One;
        Vector2 DestroyedParentSz = Vector2.Zero;
        public event Action SizeChanged;
        public event Action RelativeSizeChanged;
        #region Get/Set Size
        public Vector2 RelativeSize
        {
            get { if (Parent != null) { return Sz / Parent.Size; } else { return Vector2.One; } }
            set
            {
                // Sets the size and then updates all the children
                if (Parent == null) // If the object has no parent, set the size normally
                {
                    Size = DestroyedParentSz * value;
                }
                else
                {
                    Size = Parent.Size * value;
                }

                if (RelativeSizeChanged != null) RelativeSizeChanged();
            }
        }
        public Vector2 Size
        {
            get { return Sz; }
            set
            {
                foreach (Object2D Child in Children)
                {
                    Child.Size = value + Child.RelativeSize;
                    Child.Position = (value - Size);
                }

                Sz = value;

                if (SizeChanged != null) SizeChanged();
            }
        }
        #endregion

        #region Creation
        internal void OnDeserialize()
        {
            foreach (Object2D child in Children.ToArray())
            {
                child.Parent = this;
                child.OnDeserialize();
            }

            foreach (Component2D comp in Components.ToArray())
            {
                comp.LinkedObject = this;
                comp.OnDeserialize();
                comp.Start();
            }

            SetPivot(Al);
        }

        public Object2D(string objectName, Vector2 position, Vector2 size, float rotation = 0, Component2D[] components = null, Alignment alignment = Alignment.TopLeft, float layer = 0, Object2D parent = null, int subLayer = 1, Object2D[] children = null)
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
                Position = Parent.Position + position;
                Parent.AddChild(this, subLayer);
            }

            // Goes through all the possible errors
            if (ObjectName == "")
            {
                Debug.LogError("Object name can't be blank!", true, 3);
            }
            else if (Level.Objects2D.ContainsKey(ObjectName))
            {
                Debug.LogWarning("The 2DObject \"" + ObjectName + "\" already exists! Appending name.", true);

                List<string> Num = Level.Objects2D.Keys.ToList().FindAll((x) => x.Contains(ObjectName + "_"));
                int CurrentBiggest = 1;
                foreach (string item in Num.ToList())
                {
                    int.TryParse(item.Substring(ObjectName.Length + 1), out int num);
                    CurrentBiggest = Math.Max(num, CurrentBiggest) + 1;
                }

                ObjectName += "_" + CurrentBiggest;

                Level.Objects2D.Add(ObjectName, this);
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
            }

            if (children != null)
            {
                foreach (Object2D Child in children)
                {
                    Child.Position = Pos + Child.Position;

                    AddChild(Child);
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
                component.Awake();
                component.Start();
                if (ComponentAdded != null) ComponentAdded(component);
            }

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

        public List<T> GetComponents<T>()
        {
            List<T> Com = new List<T>();

            foreach (var item in Components.FindAll(x => x is T))
            {
                Com.Add((T)Convert.ChangeType(item, typeof(T)));
            }

            if (Com.Count > 0)
            {
                return Com;
            }
            else
            {
                Debug.LogError("2DObject \"" + ObjectName + "\" does not contain any components of type \"" + typeof(T).Name + "\"", true);
                return null;
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
            if (En)
            {
                foreach (Component2D Com in Components)
                {
                    Com.Update();
                }
            }
        }

        public virtual void FixedUpdate()
        {
            if (En)
            {
                foreach (Component2D Com in Components)
                {
                    Com.FixedUpdate();
                }
            }
        }

        public virtual void Draw()
        {
            if (En)
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
                    Child.DestroyedParentSz = Sz;
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