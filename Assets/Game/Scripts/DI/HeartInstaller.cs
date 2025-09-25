using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class HeartInstaller : MonoInstaller
    {
        [SerializeField] private Heart _heartPrefab;
        public override void InstallBindings()
        {
            Container.BindFactory<Heart, Heart.Factory>()
                .FromComponentInNewPrefab(_heartPrefab)
                .AsTransient();
        }
    }
}