using System.Collections.Generic;
using Zenject;
using UnityEngine;
using System.Linq;

namespace Asteroids
{
    public class AsteroidPool
    {
        private readonly List<AsteroidPresentation> _pool = new();
        private readonly AsteroidPresentation _prefabAsteroid;
        private readonly AsteroidPresentation _prefabBigAsteroid;
        private readonly AsteroidPresentation _prefabSmallAsteroid;
        private readonly DiContainer _container;
        private readonly PhysicsWorld _world;
        public int ActiveCount => _pool.Count(a => a.gameObject.activeSelf);

        public AsteroidPool(AsteroidPresentation prefabAsteroid, AsteroidPresentation prefabBigAsteroid, AsteroidPresentation prefabSmallAsteroid,
            int initialSize, DiContainer container, PhysicsWorld world)
        {
            _prefabAsteroid = prefabAsteroid;
            _prefabBigAsteroid = prefabBigAsteroid;
            _prefabSmallAsteroid = prefabSmallAsteroid;
            _container = container;
            _world = world;

            for (int i = 0; i < initialSize; i++)
            {
                var asteroid = _container.InstantiatePrefabForComponent<AsteroidPresentation>(GetRandomAsteroid());
                asteroid.Deactivate();
                _pool.Add(asteroid);
            }
        }

        private AsteroidPresentation GetRandomAsteroid()
        {
            if (Random.value <= 0.5)
                return _prefabAsteroid;
            else
                return _prefabBigAsteroid;
        }

        public AsteroidPresentation Get(Vector2 position, float speed)
        {
            foreach (var asteroid in _pool)
            {
                if (!asteroid.gameObject.activeSelf)
                {
                    asteroid.Activate(position, speed);
                    return asteroid;
                }
            }

            return null;
        }
        
        public AsteroidPresentation Get(Vector2 position, float speed, AsteroidType type)
        {
            // ищем свободный объект нужного типа
            foreach (var asteroid in _pool)
            {
                if (!asteroid.gameObject.activeSelf && asteroid.Type == type)
                {
                    asteroid.Activate(position, speed);
                    return asteroid;
                }
            }

            // если все заняты → создаём новый
            AsteroidPresentation prefab = type switch
            {
                AsteroidType.Small => _prefabSmallAsteroid,
                AsteroidType.Medium => _prefabAsteroid,
                AsteroidType.Large => _prefabBigAsteroid,
                _ => _prefabAsteroid
            };

            var newAsteroid = _container.InstantiatePrefabForComponent<AsteroidPresentation>(prefab);
            newAsteroid.Deactivate();
            _pool.Add(newAsteroid);

            newAsteroid.Activate(position, speed);
            return newAsteroid;
        }



        public void Release(AsteroidPresentation asteroid)
        {
            asteroid.Deactivate();
        }
    }
}