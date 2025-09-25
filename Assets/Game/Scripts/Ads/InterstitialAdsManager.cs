using System;
using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;

namespace Asteroids
{
    public class InterstitialAdsManager : MonoBehaviour
    {
        private InterstitialAdLoader _loader;
        private Interstitial _interstitial;
        private string _adUnitId = "R-M-17335296-1";

        private void Awake()
        {
            _loader = new InterstitialAdLoader();
            _loader.OnAdLoaded += HandleAdLoaded;
            _loader.OnAdFailedToLoad += HandleAdFailedToLoad;

            RequestAd();
        }

        private void OnDestroy()
        {
            _loader.OnAdLoaded -= HandleAdLoaded;
            _loader.OnAdFailedToLoad -= HandleAdFailedToLoad;
            _interstitial?.Destroy();
        }

        private void RequestAd()
        {
            var request = new AdRequestConfiguration.Builder(_adUnitId).Build();
            _loader.LoadAd(request);
        }

        public void ShowAd(Action onAdClosed)
        {
            if (_interstitial == null)
            {
                onAdClosed?.Invoke();
                RequestAd();
                return;
            }

            _interstitial.OnAdDismissed += (s, e) =>
            {
                onAdClosed?.Invoke();
                _interstitial.Destroy();
                _interstitial = null;
                RequestAd();
            };

            _interstitial.Show();
        }

        private void HandleAdLoaded(object sender, InterstitialAdLoadedEventArgs args)
        {
            _interstitial = args.Interstitial;
        }

        private void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            Debug.LogWarning($"Ad failed to load: {args.Message}");
        }
    }
}