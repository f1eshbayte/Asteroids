using System;
using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class PlayerWeapons : MonoBehaviour, IPausable
    {
        [SerializeField] private Transform _shootPoint;

        private ShipConfig _config;
        private BulletPool _bulletPool;
        private LaserPool _laserPool;
        private ShipPresentation _ship;
        private SignalBus _signalBus;
        private IShipInput _input;

        public int CurrentLaserShots { get; private set; }
        public float RechargeTimer { get; private set; }

        private bool _isPaused;

        public event Action<int> OnShootLaser;
        public event Action<float> OnChargeRecovery;

        [Inject]
        public void Construct(BulletPool bulletPool, LaserPool laserPool,
            ShipPresentation ship, ShipConfig config,
            IShipInput input, SignalBus signalBus)
        {
            _input = input;
            _config = config;
            _bulletPool = bulletPool;
            _laserPool = laserPool;
            _ship = ship;
            _signalBus = signalBus;

            RechargeTimer = _config.laserRechargeTime;
            CurrentLaserShots = _config.maxLaserShots;
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
            if (_ship.IsDie || _isPaused)
                return;

            if (_input.FireBullet)
                FireBullet();

            if (_input.FireLaser && CurrentLaserShots > 0)
                FireLaser();

            if (CurrentLaserShots < _config.maxLaserShots)
                ChargeRecovery();
        }

        private void FireBullet()
        {
            Vector2 position = _shootPoint.position;
            float rotation = _ship.transform.eulerAngles.z;
            _bulletPool.Get(position, rotation, _config.bulletSpeed);
        }

        private void ChargeRecovery()
        {
            RechargeTimer -= Time.deltaTime;

            if (RechargeTimer <= 0f)
            {
                CurrentLaserShots++;
                OnShootLaser?.Invoke(CurrentLaserShots);
                RechargeTimer = _config.laserRechargeTime;
            }

            OnChargeRecovery?.Invoke(RechargeTimer);
        }

        private void FireLaser()
        {
            CurrentLaserShots--;

            var laser = _laserPool.Get(_shootPoint.position, _config.laserLifetime);
            laser.transform.SetParent(_ship.transform, false);
            laser.transform.localPosition = _shootPoint.localPosition;
            laser.transform.localRotation = Quaternion.identity;
            OnShootLaser?.Invoke(CurrentLaserShots);
        }

        public void OnPauseChanged(PauseChangedSignal signal)
        {
            _isPaused = signal.IsPaused;
        }
    }
}