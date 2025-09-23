using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class LaserFactory
    {
        private readonly LaserPresentation _template;
        private readonly Transform _parent;
        private DiContainer _container;

        public LaserFactory(LaserPresentation template, DiContainer container, Transform parent = null)
        {
            _template = template;
            _parent = parent;
            _container = container;
        }

        public LaserPresentation Create()
        {
            var laser = Object.Instantiate(_template, _parent);
            _container.Inject(laser);
            laser.gameObject.SetActive(false);
            return laser;
        }
    }
}