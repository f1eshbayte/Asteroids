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
        private Ship ShipBody;
        private ShipConfig _config;
        private SignalBus _signalBus;
        private EnemyType _type = EnemyType.None;
        private IShipInput _input;

        private int _curentHealth;

        private bool _canCollide = false;
        private bool _isRespawning;
        private bool _isPaused = false;

        public bool IsDie { get; private set; } = false;

        public event Action<Ship, PhysicsVisual> OnShipCollided;
        public event Action GameOver;

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
            _curentHealth = _config.maxHealth;
            UniTask.Delay(TimeSpan.FromSeconds(0.1f)).ContinueWith(() => _canCollide = true);
        }

        // private void OnEnable()
        // {
        //     PauseManager.Register(this);
        // }
        //
        // private void OnDisable()
        // {
        //     PauseManager.Unregister(this);
        // }
        private void OnEnable()
        {
            _signalBus.Subscribe<PauseChangedSignal>(OnPauseChanged);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<PauseChangedSignal>(OnPauseChanged);
        }

        // private void Update()
        // {
        //     if (IsDie || _isPaused)
        //         return;
        //
        //     float deltaTime = Time.deltaTime;
        //
        //     float rotateInput =
        //         Input.GetAxis("Horizontal"); // отдельный класс или чет такое для мышки вирт джостика и клавы
        //     ShipBody.Rotate(rotateInput, deltaTime);
        //
        //     float thrustInput = Mathf.Max(0, Input.GetAxis("Vertical"));
        //     ShipBody.Thrust(thrustInput, deltaTime);
        //
        //     ShipBody.ApplyDrag(deltaTime);
        //     ShipBody.Position = transform.position;
        // }
        private void Update()
        {
            if (IsDie || _isPaused)
                return;

            float deltaTime = Time.deltaTime;
            
            ShipBody.Rotate(_input.Rotation, deltaTime);
            ShipBody.Thrust(_input.Thrust, deltaTime);
            ShipBody.ApplyDrag(deltaTime);
            ShipBody.Position = transform.position;
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
            _curentHealth--;
            Debug.Log($"HP: {_curentHealth}");
            IsDied();
        }

        private async UniTask FreezeInput(int duration)
        {
            var token = this.GetCancellationTokenOnDestroy();

            IsDie = true;
            // await UniTask.Delay(TimeSpan.FromSeconds(duration));
            await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: token);

            if (this == null || _isPaused)
                return;

            IsDie = false;
            ShipBody.Respawn(Vector2.zero, _config.godDuration);
            // Мгновенно синхронизируем визуал к новой позиции, чтобы избежать повторного столкновения
            transform.position = ShipBody.Position;

            _particle.Invincibility(_config.godDuration).Forget();
            // Разрешаем следующий респавн после установки God и запуска ауры
            _isRespawning = false;
        }

        private void IsDied()
        {
            if (_curentHealth <= 0)
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