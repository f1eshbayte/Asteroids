using System.Collections.Generic;
using Zenject;
using UnityEngine;
using System.Linq;

namespace Asteroids
{
    public class AsteroidPool
    {
        private readonly List<AsteroidView> _pool = new();
        private readonly AsteroidView _prefab;
        private readonly DiContainer _container;
        private readonly PhysicsWorld _world;
        public int ActiveCount => _pool.Count(a => a.gameObject.activeSelf);

        public AsteroidPool(AsteroidView prefab, int initialSize, DiContainer container, PhysicsWorld world)
        {
            _prefab = prefab;
            _container = container;
            _world = world;

            for (int i = 0; i < initialSize; i++)
            {
                var asteroid = _container.InstantiatePrefabForComponent<AsteroidView>(_prefab);
                asteroid.Deactivate();
                _pool.Add(asteroid);
            }
        }

        public AsteroidView Get(Vector2 position, float speed)
        {
            foreach (var asteroid in _pool)
            {
                if (!asteroid.gameObject.activeSelf)
                {
                    asteroid.Activate(position, speed);
                    return asteroid;
                }
            }

            var newAsteroid = _container.InstantiatePrefabForComponent<AsteroidView>(_prefab);
            _pool.Add(newAsteroid);
            newAsteroid.Activate(position, speed);
            return newAsteroid;
        }

        public void Release(AsteroidView asteroid)
        {
            asteroid.Deactivate();
        }
    }
}
