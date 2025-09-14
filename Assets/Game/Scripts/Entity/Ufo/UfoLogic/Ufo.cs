using UnityEngine;

namespace Asteroids
{
    public class Ufo : Body2D
    {
        private float _speed;
        public Vector2 Position;

        public Ufo(Vector2 position, float mass, float speed) : base(position, mass)
        {
            _speed = speed;
            Position = position;
        }

        
    }
}