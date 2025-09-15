using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class ShipPresentation : PhysicsVisual
    {
        [SerializeField] private int _freezeTime = 2;
        [SerializeField] private float _godDuration = 2;
        public Ship ShipBody { get; private set; }
        public bool IsDie { get; private set; }

        // public event Action<Ship, AsteroidPresentation> OnShipCollided;
        public event Action<Ship, PhysicsVisual> OnShipCollided;

        [Inject]
        public void Construct(Ship ship, PhysicsWorld world)
        {
            ShipBody = ship;
            Init(ship);
            world.Register(this);
        }

        private void Update()
        {
            if (IsDie)
                return;

            float deltaTime = Time.deltaTime;

            float rotateInput = Input.GetAxis("Horizontal");
            ShipBody.Rotate(rotateInput, deltaTime);

            float thrustInput = Mathf.Max(0, Input.GetAxis("Vertical"));
            ShipBody.Thrust(thrustInput, deltaTime);

            ShipBody.ApplyDrag(deltaTime);
            ShipBody.Position = transform.position;
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (ShipBody.IsGod)
                return;

            if (other.TryGetComponent(out AsteroidPresentation asteroid))
            {
                OnShipCollided?.Invoke(ShipBody, asteroid);
                FreezeInput(_freezeTime).Forget();
                Debug.Log("Ship collision detected!");
            }

            if (other.TryGetComponent(out UfoPresentation ufo))
            {
                OnShipCollided?.Invoke(ShipBody, ufo);
                FreezeInput(_freezeTime).Forget();
                Debug.Log("Ship collision detected!");
            }
        }

        private async UniTask FreezeInput(int duration)
        {
            IsDie = true;
            await UniTask.Delay(TimeSpan.FromSeconds(duration));

            IsDie = false;
            ShipBody.Respawn(Vector2.zero, _godDuration);
        }
    }
}