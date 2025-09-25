using Zenject;

namespace Asteroids
{
    public class MenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<Menu>().AsSingle();
        }
    }
}