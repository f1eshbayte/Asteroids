using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Asteroids
{
    [RequireComponent(typeof(ShipInvincibilityParticle))]
    public class ShipPresentation : PhysicsVisual, IPausable
    {
        private ShipInvincibilityParticle _particle;
        private ParticleHitEffectPool _poolHitEffect;
        private SignalBus _signalBus;
        private ShipConfig _config;
        private EnemyType _type = EnemyType.None;
        private IShipInput _input;


        private bool _canCollide = false;
        private bool _isRespawning;
        private bool _isPaused = false;

        public Ship ShipBody { get; private set; }
        public int MaxHealth => _config.maxHealth;
        public int CurrentHealth { get; private set; }
        public bool IsDie { get; private set; } = false;

        public event Action<Ship, PhysicsVisual> OnShipCollided;

        public event Action OnHealthChanged;
        public event Action GameOver;
        public event Action OnUpdated;

        [Inject]
        public void Construct(Ship ship, PhysicsWorld world, ParticleHitEffectPool poolHitEffect, ShipConfig config,
            IShipInput input, SignalBus signalBus)
        {
            _input = input;
            _config = config;
            ShipBody = ship;
            Init(ship, _type);
            world.Register(this);
            _particle = GetComponent<ShipInvincibilityParticle>();
            _poolHitEffect = poolHitEffect;
            _signalBus = signalBus;
        }

        private void Start()
        {
            CurrentHealth = _config.maxHealth;
            UniTask.Delay(TimeSpan.FromSeconds(0.1f)).ContinueWith(() => _canCollide = true);
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<PauseChangedSignal>(OnPauseChanged);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<PauseChangedSignal>(OnPauseChanged);
        }

        private void Update()
        {
            if (_isPaused)
                return;

            float deltaTime = Time.deltaTime;

            if (!IsDie)
            {
                ShipBody.Rotate(_input.Rotation, deltaTime);
                ShipBody.Thrust(_input.Thrust, deltaTime);
                ShipBody.Position = transform.position;
            }

            ShipBody.ApplyDrag(deltaTime);
            OnUpdated?.Invoke(); // Всегда обновляем VM
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_canCollide || ShipBody.IsGod || IsDie || _isRespawning)
                return;

            if (other.TryGetComponent(out AsteroidPresentation asteroid))
            {
                _poolHitEffect.Get(transform.position);
                OnShipCollided?.Invoke(ShipBody, asteroid);
                TakeDamage();
                if (!_isRespawning)
                {
                    _isRespawning = true;
                    FreezeInput(_config.freezeTime).Forget();
                }
            }

            if (other.TryGetComponent(out UfoPresentation ufo))
            {
                _poolHitEffect.Get(transform.position);
                OnShipCollided?.Invoke(ShipBody, ufo);
                TakeDamage();
                if (!_isRespawning)
                {
                    _isRespawning = true;
                    FreezeInput(_config.freezeTime).Forget();
                }
            }
        }

        private void TakeDamage()
        {
            if (IsDie)
                return;
            CurrentHealth--;
            OnHealthChanged?.Invoke();
            IsDied();
        }

        private async UniTask FreezeInput(int duration)
        {
            var token = this.GetCancellationTokenOnDestroy();

            IsDie = true;
            await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: token);

            if (this == null || _isPaused)
                return;

            IsDie = false;
            ShipBody.Respawn(Vector2.zero, _config.godDuration);
            transform.position = ShipBody.Position;

            _particle.Invincibility(_config.godDuration).Forget();
            _isRespawning = false;
        }

        private void IsDied()
        {
            if (CurrentHealth <= 0)
                Death();
        }

        private void Death()
        {
            GameOver?.Invoke();
        }

        public void OnPauseChanged(PauseChangedSignal signal)
        {
            _isPaused = signal.IsPaused;
        }
    }
}