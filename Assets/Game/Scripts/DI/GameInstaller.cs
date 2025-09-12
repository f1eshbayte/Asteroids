using UnityEngine;
using Zenject;

namespace Asteroids
{


    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private ShipView _shipPrefab;
        [SerializeField] private float _worldWidth = 1000f;
        [SerializeField] private float _worldHeight = 1000f;

        public override void InstallBindings()
        {
            // PhysicsWorld singleton
            Container.BindInterfacesAndSelfTo<PhysicsWorld>().AsSingle()
                .OnInstantiated<PhysicsWorld>((ctx, world) => { world.SetWorldSize(_worldWidth, _worldHeight); });

            // ShipModel
            Container.Bind<ShipModel>().AsSingle().WithArguments(
                Vector2.zero, 0.5f, 1f, 7f, 180f, 0.5f
            );

            // ShipView через Zenject
            Container.Bind<ShipView>()
                .FromComponentInNewPrefab(_shipPrefab)
                .AsSingle()
                .NonLazy();
        }

    }
}