using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class UfoPresentation : PhysicsVisual
    {
        [SerializeField] private float _mass = 1f;
        [SerializeField] private float _steerStrength = 5f;
        [SerializeField] private float _maxChaseDistance = 50f;

        private PhysicsWorld _world;
        private Ship _target;
        
        public Ufo UfoBody { get; private set; }
        
        
        [Inject]
        public void Construct(PhysicsWorld world, Ship target)
        {
            _world = world;
            _target = target;
        }

        private void Update()
        {
            if (UfoBody == null || _target == null || !gameObject.activeSelf)
                return;
            
            Vector2 toTarget = _target.Position - UfoBody.Position;
            if (toTarget.sqrMagnitude > _maxChaseDistance * _maxChaseDistance) 
                return;
            
            Vector2 desired = toTarget.normalized * UfoBody.Speed;
            // UfoBody.Velocity = Vector2.MoveTowards(UfoBody.Velocity, desired, _steerStrength * Time.deltaTime);
            UfoBody.Velocity = Vector2.Lerp(
                UfoBody.Velocity,
                desired,
                _steerStrength * Time.deltaTime
            );

            Debug.Log($"ToTarget={toTarget}, Velocity={UfoBody.Velocity}, Desired={desired}");

        }
        
        public void Activate(Vector2 position, float speed, Ship target)
        {
            if (UfoBody == null)
                UfoBody = new Ufo(position, _mass, speed);
            else
                UfoBody.Reset(position, speed);

            _target = target;
            Init(UfoBody);
            _world.Register(this);
            gameObject.SetActive(true);
        }
        
        public void Deactivate()
        {
            if (_world != null)
                _world.Unregister(this);

            _target = null;
            UfoBody = null;
            gameObject.SetActive(false);
        }

        public void OnHit(UfoPool pool)
        {
            // вызов при попадании снаряда
            pool.Release(this);
        }
    }
}