using Zenject;

namespace Asteroids
{
    public class ViewModelInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<RewardViewModel>()
                .AsSingle()
                .NonLazy();
            Container.BindInterfacesAndSelfTo<ShipCoordinatesViewModel>()
                .AsSingle()
                .NonLazy();
            Container.BindInterfacesAndSelfTo<LaserViewModel>()
                .AsSingle()
                .NonLazy();
            Container.BindInterfacesAndSelfTo<ShipHealthViewModel>()
                .AsSingle()
                .NonLazy();
        }
    }
}