using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class UfoInstaller : MonoInstaller
    {
        [SerializeField] private UfoPresentation _ufoPrefab;
        [SerializeField] private UfoSpawner _ufoSpawner;
        [SerializeField] private int _poolSize = 2;
        
        public override void InstallBindings()
        {

            Container.Bind<UfoPool>().AsSingle()
                .WithArguments(_ufoPrefab, _poolSize);

            Container.Bind<UfoFactory>().AsSingle();
            
            Container.Bind<UfoSpawner>()
                .FromComponentInNewPrefab(_ufoSpawner)
                .AsSingle()
                .NonLazy();
        }
        
    }
}