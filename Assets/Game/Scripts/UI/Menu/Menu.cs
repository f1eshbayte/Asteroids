using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class Menu
    {
        private readonly SignalBus _signalBus;

        public Menu(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        public void OpenPanelAndStopTime(GameObject panel)
        {
            panel.SetActive(true);
            _signalBus.Fire(new PauseChangedSignal(true));
        }

        public void ClosePanel(GameObject panel)
        {
            panel.SetActive(false);
            _signalBus.Fire(new PauseChangedSignal(false));
            }

        public void Exit()
        {
            Application.Quit();
        }
    }
}