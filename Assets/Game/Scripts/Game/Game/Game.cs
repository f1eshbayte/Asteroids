using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Asteroids
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private GameObject _gameOverPanel;
        
        private Menu _menu;
        private ShipPresentation _ship;

        private InterstitialAdsManager _adsManager;

        [Inject]
        public void Construct(Menu menu, ShipPresentation ship, InterstitialAdsManager adsManager)
        {
            _menu = menu;
            _ship = ship;
            _ship.GameOver += HandleGameOver;
            _adsManager = adsManager;
        }

        private void OnDisable()
        {
            _ship.GameOver -= HandleGameOver;
        }

        private void HandleGameOver()
        {
            _menu.OpenPanelAndStopTime(_gameOverPanel);
        }
        
        public void OnRestartButtonClick(GameObject panel)
        {
            _menu.ClosePanel(panel);
            // _adsManager.ShowAd(() =>
            // {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            // });
        }

        public void OnExitButtonClick()
        {
            _menu.Exit();
        }
    }
}