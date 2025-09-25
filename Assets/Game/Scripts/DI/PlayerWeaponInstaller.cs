using Zenject;

namespace Asteroids
{
    public class PlayerWeaponInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PlayerWeapons>().FromComponentInHierarchy().AsSingle();
        }
    }
}