using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class BulletFactory
    {
        private readonly BulletPresentation _template;
        private readonly Transform _parent;

        private DiContainer _container;

        public BulletFactory(BulletPresentation template, Transform parent, DiContainer container)
        {
            _template = template;
            _parent = parent;
            _container = container;
        }

        public BulletPresentation Create()
        {
            var bullet = Object.Instantiate(_template, _parent);
            _container.Inject(bullet);
            bullet.gameObject.SetActive(false);
            return bullet;
        }
    }   
}
