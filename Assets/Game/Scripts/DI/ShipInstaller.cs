using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class ShipInstaller : MonoInstaller
    {
        [SerializeField] private ShipPresentation _shipPrefab;

        [Inject] private ShipConfig _shipConfig;

        public override void InstallBindings()
        {
            Container.Bind<Ship>().AsSingle()
                .WithArguments(
                    (object)Vector2.zero,               // оборачиваем в object, чтобы Zenject не пытался резолвить
                    (object)_shipConfig.mass,
                    (object)_shipConfig.accelerationPower,
                    (object)_shipConfig.rotationSpeed,
                    (object)_shipConfig.drag
                );

            Container.Bind<ShipPresentation>()
                .FromComponentInNewPrefab(_shipPrefab)
                .AsSingle()
                .NonLazy();
        }
        
    }
}