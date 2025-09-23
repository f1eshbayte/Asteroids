using UnityEngine;

namespace Asteroids
{
    public class ParticleHitEffectFactory
    {
        private readonly ParticleSystem _template;
        private readonly Transform _parent;
            
        public ParticleHitEffectFactory(ParticleSystem template, Transform parent)
        {
            _parent = parent;
            _template = template;
        }

        public ParticleSystem Create()
        {
            var effect = Object.Instantiate(_template, _parent);
            effect.gameObject.SetActive(false);
            return effect;
        }
    }
}