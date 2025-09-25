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
        private PhysicsWorld _world;
        private IShipInput _input;
        private SignalBus _signalBus;

        private int _currentLaserShots;
        private float _rechargeTimer;
        private bool _isPaused;
            
        [Inject]
        public void Construct(BulletPool bulletPool, LaserPool laserPool, 
            ShipPresentation ship, PhysicsWorld world, ShipConfig config, IShipInput input, SignalBus signalBus)
        {
            _input = input;
            _config = config;
            _bulletPool = bulletPool;
            _laserPool = laserPool;
            _ship = ship;
            _world = world;
            _signalBus = signalBus;
            
            _currentLaserShots = _config.maxLaserShots;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<PauseChangedSignal>(OnPauseChanged);
            // PauseManager.Register(this);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<PauseChangedSignal>(OnPauseChanged);
            
            // PauseManager.Unregister(this);
        }

        // private void Update()
        // {
        //     if (_ship.IsDie || _isPaused)
        //         return;
        //
        //     if (Input.GetKeyDown(KeyCode.Space))
        //     {
        //         FireBullet();
        //     }
        //
        //     if (Input.GetKeyDown(KeyCode.LeftControl) && _currentLaserShots > 0)
        //     {
        //         FireLaser();
        //     }
        //
        //     if (_currentLaserShots < _config.maxLaserShots)
        //     {
        //         ChargeRecovery();
        //     }
        // }
        private void Update()
        {
            if (_ship.IsDie || _isPaused)
                return;
        
            if (_input.FireBullet)
                FireBullet();
        
            if (_input.FireLaser && _currentLaserShots > 0)
                FireLaser();
        
            if (_currentLaserShots < _config.maxLaserShots)
                ChargeRecovery();
        }

        private void FireBullet()
        {
            // Vector2 position = _shootPoint.position;
            Vector2 position = _shootPoint.position;
            float rotation = _ship.transform.eulerAngles.z;
            _bulletPool.Get(position, rotation, _config.bulletSpeed);
        }

        private void ChargeRecovery()
        {
            _rechargeTimer -= Time.deltaTime;
            if (_rechargeTimer <= 0f)
            {
                _currentLaserShots++;
                _rechargeTimer = _config.laserRechargeTime;
            }
        }

        private void FireLaser()
        {
            _currentLaserShots--;
            _rechargeTimer = _config.laserRechargeTime;

            var laser = _laserPool.Get(_shootPoint.position, _config.laserLifetime);
            laser.transform.SetParent(_ship.transform, false);
            laser.transform.localPosition = _shootPoint.localPosition;
            laser.transform.localRotation = Quaternion.identity;
        }

        public void OnPauseChanged(PauseChangedSignal signal)
        {
            _isPaused = signal.IsPaused;
        }
    }
}
