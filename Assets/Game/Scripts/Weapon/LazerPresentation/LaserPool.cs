using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Asteroids
{
    public class LaserPool
    {
        private readonly List<LaserPresentation> _pool = new();
        private readonly LaserFactory _factory;

        public LaserPool(LaserFactory factory, int initialSize)
        {
            _factory = factory;

            for (int i = 0; i < initialSize; i++)
            {
                var obj = _factory.Create();
                obj.Deactivate();
                _pool.Add(obj);
            }
        }

        public LaserPresentation Get(Vector2 position, float lifetime)
        {
            var laser = _pool.FirstOrDefault(l => !l.gameObject.activeSelf);

            if (laser == null)
            {
                laser = _factory.Create();
                _pool.Add(laser);
            }

            var body = new Laser(position, lifetime);
            laser.Init(body, lifetime);

            return laser;
        }

        public void SubscribeAllLasers(Action<Laser, PhysicsVisual> callback)
        {
            foreach (var laser in _pool)
                laser.OnLaserCollided += callback;
        }

        public void UnsubscribeAllLasers(Action<Laser, PhysicsVisual> callback)
        {
            foreach (var laser in _pool)
                laser.OnLaserCollided -= callback;
        }
    }
}
