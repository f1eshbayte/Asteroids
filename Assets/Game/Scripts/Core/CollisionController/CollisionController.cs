using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class CollisionController : MonoBehaviour
    {
        private RewardSystem _rewardSystem;
        
        private AsteroidFactory _asteroidFactory;
        private ShipPresentation _shipPresentation;
        private AsteroidPool _asteroidPool;
        private BulletPool _bulletPool;
        private UfoPool _ufoPool;
        private LaserPool _laserPool;

        [Inject]
        public void Construct
        (
            AsteroidFactory asteroidFactory,
            ShipPresentation shipPresentation,
            AsteroidPool asteroidPool,
            UfoPool ufoPool,
            BulletPool bulletPool,
            LaserPool laserPool,
            RewardSystem rewardSystem
        )
        {
            _rewardSystem = rewardSystem;
            _asteroidFactory = asteroidFactory;
            _asteroidPool = asteroidPool;
            _shipPresentation = shipPresentation;
            _bulletPool = bulletPool;
            _ufoPool = ufoPool;
            _laserPool = laserPool;

            _shipPresentation.OnShipCollided += HandleShipCollision;
            _bulletPool.SubscribeAllBullets(HandleBulletCollision);
            _laserPool.SubscribeAllLasers(HandleLaserCollision);
        }

        private void HandleShipCollision(Ship ship, PhysicsVisual target)
        {
            CollisionResolver.Resolve(ship, target, _asteroidFactory, _asteroidPool, _ufoPool);
            _rewardSystem.AddScore(target.Type);
        }

        private void HandleBulletCollision(Bullet bullet, PhysicsVisual target)
        {
            CollisionResolver.Resolve(bullet, target, _asteroidFactory, _asteroidPool, _ufoPool);
            _rewardSystem.AddScore(target.Type);
        }

        private void HandleLaserCollision(Laser laser, PhysicsVisual target)
        {
            CollisionResolver.Resolve(laser, target, _asteroidFactory, _asteroidPool, _ufoPool);
            _rewardSystem.AddScore(target.Type);
        }

        private void OnDestroy()
        {
            _shipPresentation.OnShipCollided -= HandleShipCollision;
            _bulletPool.UnsubscribeAllBullets(HandleBulletCollision);
            _laserPool.UnsubscribeAllLasers(HandleLaserCollision);
        }
    }
}