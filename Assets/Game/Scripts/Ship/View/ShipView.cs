using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class ShipView : PhysicsView
    {
        private ShipModel _ship;

        [Inject]
        public void Construct(ShipModel ship, PhysicsWorld world)
        {
            _ship = ship;
            Init(ship);
            world.Register(this);
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            float rotateInput = Input.GetAxis("Horizontal");
            _ship.Rotate(rotateInput, deltaTime);

            float thrustInput = Mathf.Max(0, Input.GetAxis("Vertical"));
            _ship.Thrust(thrustInput, deltaTime);

            _ship.ApplyDrag(deltaTime);
        }
    }
}
