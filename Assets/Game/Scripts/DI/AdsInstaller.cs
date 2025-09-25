using UnityEngine;
using Zenject;

namespace Asteroids
{
    
    public class AdsInstaller : MonoInstaller
    {
        [SerializeField] private InterstitialAdsManager _adsManagerPrefab;

        public override void InstallBindings()
        {
            BindAdsManager();
        }

        private void BindAdsManager()
        {
            Container.Bind<InterstitialAdsManager>().FromComponentInNewPrefab(_adsManagerPrefab).AsSingle().NonLazy();
        }
    }
}