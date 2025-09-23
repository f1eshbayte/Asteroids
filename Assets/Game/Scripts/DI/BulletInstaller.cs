using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class BulletInstaller : MonoInstaller
    {
        [SerializeField] private BulletPresentation _bulletPrefab;
        [SerializeField] private int _poolSize = 20;

        public override void InstallBindings()
        {
            // создаём пустой объект для пуль
            var bulletParent = new GameObject("BulletsContainer").transform;

            Container.Bind<BulletFactory>()
                .AsSingle()
                .WithArguments(_bulletPrefab, bulletParent);

            Container.Bind<BulletPool>()
                .AsSingle()
                .WithArguments(_poolSize);
        }
    }

}