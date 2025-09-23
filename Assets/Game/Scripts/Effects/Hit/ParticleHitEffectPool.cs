using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Asteroids
{
    public class ParticleHitEffectPool
    {
        private readonly List<ParticleSystem> _pool = new();
        private readonly ParticleHitEffectFactory _factory;
        
        public ParticleHitEffectPool(ParticleHitEffectFactory factory, int initialSize)
        {
            _factory = factory;

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
            effect.Play();

            // var duration = effect.main.duration + effect.main.startLifetime.constantMax;
            var duration = effect.main.startLifetime.constantMax;
            ReturnToPool(effect, duration);

            return effect;
        }

        private async UniTask ReturnToPool(ParticleSystem effect, float delay)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            effect.Stop();
            effect.gameObject.SetActive(false);
            
        }
    }
}