using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class PlayerWeapons : MonoBehaviour
    {
        // [SerializeField] private float _bulletSpeed = 15f;
        [SerializeField] private Transform _shootPoint;
        // [SerializeField] private int _maxLaserShots = 3;
        // [SerializeField] private float _laserLifetime = 0.3f;
        // [SerializeField] private float _laserRechargeTime = 5f;

        private ShipConfig _config;
        private BulletPool _bulletPool;
        private LaserPool _laserPool;
        private ShipPresentation _ship;
        private PhysicsWorld _world;

        private int _currentLaserShots;
        private float _rechargeTimer;
            
        [Inject]
        public void Construct(BulletPool bulletPool, LaserPool laserPool, ShipPresentation ship, PhysicsWorld world, ShipConfig config)
        {
            _config = config;
            _bulletPool = bulletPool;
            _laserPool = laserPool;
            _ship = ship;
            _world = world;
            
            // _currentLaserShots = _maxLaserShots;
            _currentLaserShots = _config.maxLaserShots;
        }

        private void Update()
        {
            if (_ship.IsDie)
                return;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                FireBullet();
            }

            if (Input.GetKeyDown(KeyCode.LeftControl) && _currentLaserShots > 0)
            {
                FireLaser();
            }

            if (_currentLaserShots < _config.maxLaserShots)
            {
                ChargeRecovery();
            }
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
    }
}
