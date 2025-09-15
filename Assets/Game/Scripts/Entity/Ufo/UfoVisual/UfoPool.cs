using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class UfoPool
    {
        private readonly List<UfoPresentation> _pool = new();
        private readonly UfoPresentation _prefab;
        private readonly DiContainer _container;
        private readonly PhysicsWorld _world;

        public int ActiveCount => _pool.Count(p => p.gameObject.activeSelf);

        [Inject]
        public UfoPool(UfoPresentation prefab, int initialSize, DiContainer container, PhysicsWorld world)
        {
            _prefab = prefab;
            _container = container;
            _world = world;

            for (int i = 0; i < initialSize; i++)
            {
                var obj = _container.InstantiatePrefabForComponent<UfoPresentation>(_prefab);
                obj.Deactivate();
                _pool.Add(obj);
            }
        }

        public UfoPresentation Get(Vector2 position, float speed, Ship target)
        {
            foreach (var u in _pool)
            {
                if (!u.gameObject.activeSelf)
                {
                    u.Activate(position, speed, target);
                    return u;
                }
            }

            var newU = _container.InstantiatePrefabForComponent<UfoPresentation>(_prefab);
            newU.Deactivate();
            _pool.Add(newU);

            newU.Activate(position, speed, target);
            return newU;
        }

        public void Release(UfoPresentation ufo)
        {
            ufo.Deactivate();
        }
    }
}