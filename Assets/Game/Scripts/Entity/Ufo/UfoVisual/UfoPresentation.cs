using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class UfoPresentation : PhysicsVisual
    {
        [SerializeField] private EnemyType _type;

        private UfoConfig _config;
        private PhysicsWorld _world;
        private Ship _target;
        
        public Ufo UfoBody { get; private set; }
     
        [Inject]
        public void Construct(PhysicsWorld world, Ship target, UfoConfig config)
        {
            _world = world;
            _target = target;
            _config = config;
        }

        private void FixedUpdate()
        {
            if (UfoBody == null || _target == null || !gameObject.activeSelf)
                return;
            
            Vector2 toTarget = _target.Position - UfoBody.Position;
            float distanceToTarget = toTarget.magnitude;
            
            // Если корабль слишком далеко, прекращаем преследование
            if (distanceToTarget > _config.maxChaseDistance) 
                return;
            
            // Более агрессивное преследование - UFO направляется прямо к цели
            Vector2 desired = toTarget.normalized * UfoBody.Speed;
            
            UfoBody.Velocity = Vector2.MoveTowards(
                UfoBody.Velocity,
                desired,
                _config.steerStrength * Time.fixedDeltaTime
            );
        }
        
        public void Activate(Vector2 position, float speed, Ship target)
        {
            if (UfoBody == null)
                UfoBody = new Ufo(position, _config.mass, speed);
            else
                UfoBody.Reset(position, speed);

            _target = target;
            Init(UfoBody, _type);
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