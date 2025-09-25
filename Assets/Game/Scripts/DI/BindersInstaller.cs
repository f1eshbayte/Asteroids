using MVVM;
using Zenject;

namespace Asteroids
{
    public class BindersInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BinderFactory.RegisterBinder<TextBinder>();
            BinderFactory.RegisterBinder<HealthBinder>();
            BinderFactory.RegisterBinder<MaxHealthBinder>();
        }
    }
}
