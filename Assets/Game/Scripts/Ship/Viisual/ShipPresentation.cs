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
        public Ship Ship { get; private set; }
        public bool IsDie { get; private set; }

        public event Action<Ship, AsteroidPresentation> OnShipCollided;

        [Inject]
        public void Construct(Ship ship, PhysicsWorld world)
        {
            Ship = ship;
            Init(ship);
            world.Register(this);
        }

        private void Update()
        {
            if (IsDie)
                return;

            float deltaTime = Time.deltaTime;

            float rotateInput = Input.GetAxis("Horizontal");
            Ship.Rotate(rotateInput, deltaTime);

            float thrustInput = Mathf.Max(0, Input.GetAxis("Vertical"));
            Ship.Thrust(thrustInput, deltaTime);

            Ship.ApplyDrag(deltaTime);
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out AsteroidPresentation asteroid))
            {
                if (Ship.IsGod)
                    return;
                OnShipCollided?.Invoke(Ship, asteroid);
                FreezeInput(_freezeTime).Forget();
                Debug.Log("Ship collision detected!");
            }
        }

        private async UniTaskVoid FreezeInput(int duration)
        {
            IsDie = true;
            await UniTask.Delay(TimeSpan.FromSeconds(duration));

            IsDie = false;
            Ship.Respawn(Vector2.zero, 3f);
        }
    }
}