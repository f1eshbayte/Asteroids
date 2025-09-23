using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Asteroids
{
    public class ShipInvincibilityParticle : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _invincibilityParticle;

        private void Awake()
        {
            _invincibilityParticle.Stop();
        }

        public async UniTask Invincibility(int duration)
        {
            _invincibilityParticle.Play();

            await UniTask.Delay(TimeSpan.FromSeconds(duration));

            _invincibilityParticle.Stop();
        }
    }
}