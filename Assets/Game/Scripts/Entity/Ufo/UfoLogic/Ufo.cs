using UnityEngine;

namespace Asteroids
{
    public class Ufo : Body2D
    {
        public float Speed;
        public Vector2 Position;

        public Ufo(Vector2 position, float mass, float speed) : base(position, mass)
        {
            Speed = speed;
            Position = position;
        }

        public void Reset(Vector2 position, float speed)
        {
            Position = position;
            Speed = speed;
            SetInitialVelocity();
        }

        private void SetInitialVelocity()
        {
            // random
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            Velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * Speed;
        }
        
    }
}