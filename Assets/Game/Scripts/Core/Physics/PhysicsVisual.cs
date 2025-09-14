using System;
using UnityEngine;

namespace Asteroids
{
    [RequireComponent(/*typeof(Collider2D),*/ typeof(Rigidbody2D), typeof(CircleCollider2D))]
    public class PhysicsVisual : MonoBehaviour
    {
        public Body2D Body { get; private set; }
        private Rigidbody2D _rb;
        private CircleCollider2D _circleCollider;

        public void Init(Body2D body)
        {
            Body = body;
            _circleCollider = GetComponent<CircleCollider2D>();
            _rb = GetComponent<Rigidbody2D>();
            _rb.bodyType = RigidbodyType2D.Kinematic;
        }

        public void SyncTransform()
        {
            transform.position = Body.Position;
            if (Body is Ship ship)
                transform.rotation = Quaternion.Euler(0, 0, ship.Rotation);
        }
    }
}