using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class AsteroidInstaller : MonoInstaller
    {
        [SerializeField] private AsteroidView _asteroidPrefab;
        [SerializeField] private int _poolSize = 10;
        [SerializeField] private AsteroidSpawner _spawnerPrefab;

        public override void InstallBindings()
        {
            Container.Bind<AsteroidPool>().AsSingle()
                .WithArguments(_asteroidPrefab, _poolSize);

            Container.Bind<AsteroidFactory>().AsSingle();

            Container.Bind<AsteroidSpawner>()
                .FromComponentInNewPrefab(_spawnerPrefab)
                .AsSingle()
                .NonLazy();
        }
    }
}