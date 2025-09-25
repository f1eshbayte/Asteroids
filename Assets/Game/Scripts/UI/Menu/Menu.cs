using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Asteroids
{
    public class Menu
    {
        // public static event UnityAction<bool> OnPauseStateChanged;
        private readonly SignalBus _signalBus;

        public Menu(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        public void OpenPanelAndStopTime(GameObject panel)
        {
            panel.SetActive(true);
            // OnPauseStateChanged?.Invoke(true);
            _signalBus.Fire(new PauseChangedSignal(true));
        }

        public void ClosePanel(GameObject panel)
        {
            panel.SetActive(false);
            // OnPauseStateChanged?.Invoke(false);
            _signalBus.Fire(new PauseChangedSignal(false));
            }

        public void Exit()
        {
            Application.Quit();
        }
    }
}