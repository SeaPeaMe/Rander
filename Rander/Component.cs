namespace Rander
{
    public class Component
    {
        public GameObject LinkedObject;

        public virtual void Start() { }
        public virtual void Update() { }
        public virtual void Draw() { }
    }
}
