using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class LaserInstaller : MonoInstaller
    {
        [SerializeField] private LaserPresentation _laserPrefab;
        [SerializeField] private int _poolSize = 1;

        public override void InstallBindings()
        {
            var laserParent = new GameObject("LasersContainer").transform;

            Container.Bind<LaserFactory>()
                .AsSingle()
                .WithArguments(_laserPrefab, laserParent);

            Container.Bind<LaserPool>()
                .AsSingle()
                .WithArguments(_poolSize);
        }
    }
}