namespace Rander
{
    public class Component
    {
        public virtual void Start() { }
        public virtual void Update() { }
        public virtual void Draw() { }
        public virtual void OnDispose() { }
        public virtual void OnDeserialize() { }
    }
}
