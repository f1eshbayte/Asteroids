using UnityEngine;

namespace Asteroids
{
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class PhysicsView : MonoBehaviour
    {
        public Body2D Body { get; private set; }
        private Rigidbody2D _rb;

        public void Init(Body2D body)
        {
            Body = body;
            _rb = GetComponent<Rigidbody2D>();
            _rb.bodyType = RigidbodyType2D.Kinematic;
        }

        public void SyncTransform()
        {
            transform.position = Body.Position;
            if (Body is ShipModel ship)
                transform.rotation = Quaternion.Euler(0, 0, ship.Rotation);
        }
    }
}