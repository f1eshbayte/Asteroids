using UnityEngine;

namespace Asteroids
{
    public class Ufo : Body2D
    {
        public float Speed;

        public Ufo(Vector2 position, float mass, float speed) : base(position, mass)
        {
            Speed = speed;
        }

        public void Reset(Vector2 position, float speed)
        {
            Position = position;
            Speed = speed;
            SetInitialVelocity();
        }

        private void SetInitialVelocity()
        {
            Velocity = Vector2.zero;
        }
        
    }
}