using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class InputInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                Container.Bind<VirtualJoystick>().FromComponentInHierarchy().AsSingle();
                Container.Bind<MobileFireButtons>().FromComponentInHierarchy().AsSingle();
                Container.Bind<IShipInput>().To<MobileShipInput>().AsSingle();
            }
            else
            {
                Container.Bind<IShipInput>().To<DesktopShipInput>().AsSingle();
            }
        }

    }
}