using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class ParticleHitEffectPool : IPausable
    {
        private readonly List<ParticleSystem> _pool = new();
        private readonly ParticleHitEffectFactory _factory;

        private SignalBus _signalBus;
        
        private bool _isPaused;
        
        public ParticleHitEffectPool(ParticleHitEffectFactory factory, int initialSize, SignalBus signalBus)
        {
            _factory = factory;
            _signalBus = signalBus;

            for (int i = 0; i < initialSize; i++)
            {
                var obj = _factory.Create();
                _pool.Add(obj);
            }
        }

        public ParticleSystem Get(Vector3 position)
        {
            var effect = _pool.FirstOrDefault(e => !e.gameObject.activeSelf);

            if (effect == null)
            {
                effect = _factory.Create();
                _pool.Add(effect);
            }

            effect.transform.position = position;
            effect.gameObject.SetActive(true);
            if (!_isPaused)
                effect.Play();

            var duration = effect.main.startLifetime.constantMax;
            ReturnToPool(effect, duration);

            return effect;
        }

        private async UniTask ReturnToPool(ParticleSystem effect, float delay)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: effect.GetCancellationTokenOnDestroy());
            
            if (effect != null) // на всякий случай
            {
                effect.Stop();
                effect.gameObject.SetActive(false);
            }
            
        }

        public void OnPauseChanged(PauseChangedSignal signal)
        {
            _isPaused = signal.IsPaused;
            foreach (var ps in _pool)
            {
                if (ps == null) continue;
                if (_isPaused)
                    ps.Pause();
                else if (ps.gameObject.activeSelf)
                    ps.Play();
            }
        }
    }
}