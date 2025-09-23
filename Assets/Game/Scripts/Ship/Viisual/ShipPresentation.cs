using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Asteroids
{
    [RequireComponent(typeof(ShipInvincibilityParticle))]
    public class ShipPresentation : PhysicsVisual
    {
        // [SerializeField] private int _freezeTime = 2;
        // [SerializeField] private int _godDuration = 2;
        // [SerializeField] private int _maxHealth = 3;

        private ShipInvincibilityParticle _particle;
        private ParticleHitEffectPool _poolHitEffect;
        private Ship ShipBody;
        private ShipConfig _config;
        private EnemyType _type = EnemyType.None;

        private bool _canCollide = false;
        private int _curentHealth;
        
        public bool IsDie { get; private set; } = false;

        public event Action<Ship, PhysicsVisual> OnShipCollided;

        [Inject]
        public void Construct(Ship ship, PhysicsWorld world, ParticleHitEffectPool poolHitEffect, ShipConfig config)
        {
            _config = config;
            ShipBody = ship;
            Init(ship, _type);
            world.Register(this);
            _particle = GetComponent<ShipInvincibilityParticle>();
            _poolHitEffect = poolHitEffect;
        }

        private void Start()
        {
            // _curentHealth = _maxHealth;
            _curentHealth = _config.maxHealth;
            UniTask.Delay(TimeSpan.FromSeconds(0.1f)).ContinueWith(() => _canCollide = true);
        }

        private void Update()
        {
            if (IsDie)
                return;

            float deltaTime = Time.deltaTime;

            float rotateInput = Input.GetAxis("Horizontal"); // отдельный класс или чет такое для мышки вирт джостика и клавы
            ShipBody.Rotate(rotateInput, deltaTime);

            float thrustInput = Mathf.Max(0, Input.GetAxis("Vertical"));
            ShipBody.Thrust(thrustInput, deltaTime);

            ShipBody.ApplyDrag(deltaTime);
            ShipBody.Position = transform.position;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_canCollide || ShipBody.IsGod)
                return;

            if (other.TryGetComponent(out AsteroidPresentation asteroid))
            {
                _poolHitEffect.Get(transform.position);
                OnShipCollided?.Invoke(ShipBody, asteroid);
                // FreezeInput(_freezeTime).Forget();
                FreezeInput(_config.freezeTime).Forget();
            }

            if (other.TryGetComponent(out UfoPresentation ufo))
            {
                _poolHitEffect.Get(transform.position);
                OnShipCollided?.Invoke(ShipBody, ufo);
                // FreezeInput(_freezeTime).Forget();
                FreezeInput(_config.freezeTime).Forget();
            }
            
            TakeDamage();
        }

        private void TakeDamage()
        {
            _curentHealth--;
        }

        private async UniTask FreezeInput(int duration)
        {
            IsDie = true;
            await UniTask.Delay(TimeSpan.FromSeconds(duration));

            IsDie = false;
            // ShipBody.Respawn(Vector2.zero, _godDuration);
            ShipBody.Respawn(Vector2.zero, _config.godDuration);

            // _particle.Invincibility(_godDuration).Forget();
            _particle.Invincibility(_config.godDuration).Forget();
        }
    }
}