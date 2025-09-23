using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Asteroids
{
    public class BulletPool
    {
        private readonly List<BulletPresentation> _pool = new();
        private readonly BulletFactory _factory;
        private readonly PhysicsWorld _world;

        public BulletPool(BulletFactory factory, int capacity, PhysicsWorld world)
        {
            _factory = factory;
            _world = world;

            for (int i = 0; i < capacity; i++)
                _pool.Add(_factory.Create());
        }

        public BulletPresentation Get(Vector2 position, float rotation, float speed, float lifetime = 2f)
        {
            var bullet = _pool.FirstOrDefault(b => !b.gameObject.activeSelf);

            if (bullet == null)
            {
                bullet = _factory.Create();
                _pool.Add(bullet);
            }

            float radian = rotation * Mathf.Deg2Rad;
            Vector2 velocity = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)) * speed;

            var body = new Bullet(position, velocity, lifetime);
            bullet.InitBullet(body, _world);

            return bullet;
        }
        
        public void SubscribeAllBullets(Action<Bullet, PhysicsVisual> callback)
        {
            foreach (var bullet in _pool)
            {
                bullet.OnBulletCollided += callback;
            }
        }

        public void UnsubscribeAllBullets(Action<Bullet, PhysicsVisual> callback)
        {
            foreach (var bullet in _pool)
            {
                bullet.OnBulletCollided -= callback;
            }
        }
    }
}