using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class ShipInvincibilityParticle : MonoBehaviour, IPausable
    {
        [SerializeField] private ParticleSystem _invincibilityParticle;

        private SignalBus _signalBus;
        
        private bool _isPaused;
        private bool _wasPlayingBeforePause;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<PauseChangedSignal>(OnPauseChanged);
            // PauseManager.Register(this);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<PauseChangedSignal>(OnPauseChanged);
            // PauseManager.Unregister(this);
        }

        private void Awake()
        {
            _invincibilityParticle.Stop();
        }

        public async UniTask Invincibility(int duration)
        {
            if (_invincibilityParticle == null)
                return;

            if (!_isPaused)
                _invincibilityParticle.Play();

            await UniTask.Delay(duration * 1000, 
                cancellationToken: this.GetCancellationTokenOnDestroy());
            
            if (_invincibilityParticle != null)
                _invincibilityParticle.Stop();
        }

        public void OnPauseChanged(PauseChangedSignal  signal)
        {
            _isPaused = signal.IsPaused;
            if (_invincibilityParticle == null)
                return;

            if (_isPaused)
            {
                _wasPlayingBeforePause = _invincibilityParticle.isPlaying;
                if (_wasPlayingBeforePause)
                    _invincibilityParticle.Pause();
            }
            else
            {
                if (_wasPlayingBeforePause && _invincibilityParticle.isPaused)
                    _invincibilityParticle.Play();
                _wasPlayingBeforePause = false;
            }
        }
    }
}