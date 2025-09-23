using Zenject;

namespace Asteroids
{
    public class GameInstaller : MonoInstaller
    {
        [Inject] private WorldMapConfig _config;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PhysicsWorld>().AsSingle()
                .OnInstantiated<PhysicsWorld>((ctx, world) => { world.SetWorldSize(_config.worldWidth, _config.worldHeight); });
            Container.Bind<RewardSystem>().FromInstance(new RewardSystem()).AsSingle();
        }
    }
}