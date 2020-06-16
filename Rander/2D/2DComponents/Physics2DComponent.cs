using Microsoft.Xna.Framework;
using System;

namespace Rander._2D
{
    public class Physics2DComponent : Component2D
    {
        // -------------------------
        // | CALL PROFESSOR REWORK |
        // -------------------------

        public Vector2 Velocity = Vector2.Zero;

        public float MaxFallSpeed = 30;
        public float GravityForce = 1f;
        public float Bounciness = 0.2f;
        public bool UseGravity = true;

        public Physics2DComponent(bool useGravity = true) { UseGravity = useGravity; }

        bool StopForces = false;

        public override void Start()
        {
            if (UseGravity == false)
            {
                GravityForce = 0;
            }

            Game.Physics2D.Add(this);
        }

        public override void Update()
        {
            if (Velocity.Y < MaxFallSpeed)
            {
                Velocity.Y += GravityForce * Time.FrameTime * 25;
            }

            // Check for collisions
            foreach (Physics2DComponent com in Game.Physics2D.ToArray())
            {
                if (com != this)
                {
                    if (Rectangle.Intersect(new Rectangle(LinkedObject.Position.ToPoint() - LinkedObject.Pivot.ToPoint(), LinkedObject.Size.ToPoint()), new Rectangle(com.LinkedObject.Position.ToPoint() - com.LinkedObject.Pivot.ToPoint(), com.LinkedObject.Size.ToPoint())) != Rectangle.Empty)
                    {
                        // Prevents jittering when at low velocities and colliding (TODO)
                        if (Math.Abs(Velocity.X) < 1f && Math.Abs(Velocity.Y) < 1f)
                        {
                            Velocity = Vector2.Zero;
                            StopForces = true;
                        }
                        else
                        {
                            StopForces = false;
                            Vector2 StorVel = Velocity;
                            Velocity = Vector2.Zero;
                            Velocity -= (StorVel + com.Velocity) * Bounciness * 2;
                        }
                    }
                }
            }

            if (StopForces)
            {
                Velocity = Vector2.Zero;
                StopForces = false;
            }

            LinkedObject.Position += Velocity * Time.FrameTime * 25;
        }
    }
}
