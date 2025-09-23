using UnityEngine;

namespace Asteroids
{
    public class Bullet : Body2D
    {
        public float Lifetime { get; private set; }
        private float _timer;

        public Bullet(Vector2 position, Vector2 velocity, float mass = 1f, float lifetime = 2f)
            : base(position, mass)
        {
            Velocity = velocity;
            Lifetime = lifetime;
            _timer = lifetime;
        }

        public bool UpdateLifetime(float deltaTime)
        {
            _timer -= deltaTime;
            return _timer > 0f;
        }
    }
}
