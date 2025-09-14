using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class AsteroidInstaller : MonoInstaller
    {
        [SerializeField] private AsteroidPresentation _asteroidPrefab;
        [SerializeField] private AsteroidPresentation _bigAsteroidPrefab;
        [SerializeField] private AsteroidPresentation _smallAsteroidPrefab;
        [SerializeField] private int _poolSize;
        [SerializeField] private AsteroidSpawner _spawnerPrefab;

        public override void InstallBindings()
        {
            Container.Bind<AsteroidPool>().AsSingle()
                .WithArguments(_asteroidPrefab, _bigAsteroidPrefab, _smallAsteroidPrefab, _poolSize);

            Container.Bind<AsteroidFactory>().AsSingle();

            Container.Bind<AsteroidSpawner>()
                .FromComponentInNewPrefab(_spawnerPrefab)
                .AsSingle()
                .NonLazy();
        }
    }
}