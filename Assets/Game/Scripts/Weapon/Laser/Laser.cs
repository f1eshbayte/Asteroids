using UnityEngine;

namespace Asteroids
{
    public class Laser : Body2D
    {
        public float Lifetime { get; private set; }
        private float _timer;
        
        public Laser(Vector2 position, float mass = 1f, float lifetime = 0.3f) : base(position, mass)
        {
            Lifetime = lifetime;
            _timer = lifetime;
        }
    }
}