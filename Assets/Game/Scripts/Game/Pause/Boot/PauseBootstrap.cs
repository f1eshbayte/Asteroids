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
                _signalBus.Subscribe<PauseChangedSignal>(OnPauseChanged);
            }
        }

        private void OnDisable()
        {
            if (_physicsWorld != null)
            {
                _signalBus.Unsubscribe<PauseChangedSignal>(OnPauseChanged);
            }
        }

       
        private void OnPauseChanged(PauseChangedSignal signal)
        {
            if (_tickableManager != null)
            {
                _tickableManager.IsPaused = signal.IsPaused;
            }

            if (_physicsWorld != null)
            {
                _physicsWorld.OnPauseChanged(signal);
            }
        }
    }
}


