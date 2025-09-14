using UnityEngine;
using Zenject;

namespace Asteroids
{


    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private ShipPresentation _shipPrefab;
        [SerializeField] private float _worldWidth = 10f;
        [SerializeField] private float _worldHeight = 10f;

        [Header("Ship")]
        [SerializeField] private float _mass = 1f;
        [SerializeField] private float _accelerationPower = 7f;
        [SerializeField] private float _rotationSpeed = 180f;
        [SerializeField] private float _drag = 0.5f;
        public override void InstallBindings()
        {
            // PhysicsWorld singleton
            Container.BindInterfacesAndSelfTo<PhysicsWorld>().AsSingle()
                .OnInstantiated<PhysicsWorld>((ctx, world) => { world.SetWorldSize(_worldWidth, _worldHeight); });

            // ShipModel
            Container.Bind<Ship>().AsSingle().WithArguments(Vector2.zero, _mass, _accelerationPower, _rotationSpeed, _drag);

            // ShipView через Zenject
            Container.Bind<ShipPresentation>()
                .FromComponentInNewPrefab(_shipPrefab)
                .AsSingle()
                .NonLazy();

            // GameController
            // Container.Bind<GameController>().AsSingle().NonLazy();
        }

    }
}