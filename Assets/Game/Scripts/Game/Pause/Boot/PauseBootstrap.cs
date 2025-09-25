using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class PauseBootstrap : MonoBehaviour
    {
        private PhysicsWorld _physicsWorld;
        private TickableManager _tickableManager;
        private SignalBus _signalBus;
        
        [Inject]
        public void Construct(PhysicsWorld physicsWorld, SignalBus signalBus, [InjectOptional] TickableManager tickableManager)
        {
            _physicsWorld = physicsWorld;
            _tickableManager = tickableManager;
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            if (_physicsWorld != null)
            {
                // PauseManager.Register(_physicsWorld);
                _signalBus.Subscribe<PauseChangedSignal>(OnPauseChanged);
            }
            // Menu.OnPauseStateChanged += OnPauseChanged;
        }

        private void OnDisable()
        {
            if (_physicsWorld != null)
            {
                // PauseManager.Unregister(_physicsWorld);
                _signalBus.Unsubscribe<PauseChangedSignal>(OnPauseChanged);
            }
            // Menu.OnPauseStateChanged -= OnPauseChanged;
        }

        // private void OnPauseChanged(bool isPaused)
        // {
        //     if (_tickableManager != null)
        //     {
        //         _tickableManager.IsPaused = isPaused;
        //     }
        // }
        private void OnPauseChanged(PauseChangedSignal signal)
        {
            // Например, физический мир реагирует на паузу
            if (_tickableManager != null)
            {
                _tickableManager.IsPaused = signal.IsPaused;
            }

            if (_physicsWorld != null)
            {
                // Можно добавить метод Pause/Resume в PhysicsWorld
                _physicsWorld.OnPauseChanged(signal);
            }
        }
    }
}


