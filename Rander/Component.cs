namespace Rander
{
    public class Component
    {
        public virtual void Awake() { }
        public virtual void Start() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void Draw() { }
        public virtual void OnDispose() { }
        public virtual void OnDeserialize() { }
    }
}
