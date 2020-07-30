using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Rander._3D
{
    public class Object3D
    {
        public string ObjectName;
        public Object3D Parent;
        public bool Enabled = true;

        public Matrix WorldMatrix;

        Vector3 Pos;
        public Vector3 Position { get { return Pos; } set { Pos = value; } }
        Vector3 DestroyedParentPos;

        Vector3 Rot;
        public Vector3 Rotation { get { return Rot; } set { Rot = value; } }
        Vector3 DestroyedParentRot;

        Vector3 Sz;
        public Vector3 Size { get { return Sz; } set { Sz = value; } }
        Vector3 DestroyedParentSz;

        public List<Component3D> Components = new List<Component3D>();
        #region Components
        public Component3D AddComponent(Component3D component)
        {
            if (component != null)
            {
                Components.Add(component);
                component.LinkedObject = this;
            }

            return component;
        }

        public bool HasComponent<T>()
        {
            Component3D com = Components.Find(x => x is T);
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
                Debug.LogError("3DObject \"" + ObjectName + "\" does not contain the component \"" + typeof(T).Name + "\"", true);
                return (T)Convert.ChangeType(null, typeof(T));
            }
        }

        public void RemoveComponent<T>()
        {
            Component3D Com = Components.Find(x => x is T);

            if (Com != null)
            {
                Components.Remove(Com);
            }
            else
            {
                Debug.LogError("3DObject \"" + ObjectName + "\" does not already contain the component \"" + typeof(T).Name + "\"");
            }
        }
        #endregion

        public List<Object3D> Children = new List<Object3D>();
        #region Children
        public void AddChild(Object3D object3D)
        {
            Children.Add(object3D);
        }

        public void RemoveChild(Object3D object3D)
        {
            Children.Remove(object3D);
        }
        #endregion

        #region Creation
        public Object3D(string objectName, Vector3 position, Vector3 size, Vector3 rotation, Component3D[] components = null, Object3D parent = null)
        {
            Size = size;
            Position = position;
            Rotation = rotation;

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
                Level.Objects3D.Add(ObjectName, this);
            }

            // Adds and Starts the scripts
            if (components != null)
            {
                foreach (Component3D com in components)
                {
                    AddComponent(com);
                }

                // Starts all the components after they've all been added
                foreach (Component3D com in Components)
                {
                    com.Start();
                }
            }
        }
        #endregion

        public virtual void Update()
        {
            if (Enabled)
            {
                foreach (Component3D Com in Components)
                {
                    Com.Update();
                }
            }
        }

        public virtual void Draw()
        {
            if (Enabled)
            {
                foreach (Component3D Com in Components)
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
                foreach (Object3D Child in Children)
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
                foreach (Object3D Child in Children)
                {
                    Child.Dispose();
                }
            }

            // Runs OnDispose on all scripts
            foreach (Component3D Scr in Components)
            {
                Scr.OnDispose();
            }

            // Completely un-links the object
            Level.Objects3D.Remove(ObjectName);
        }
    }
}
