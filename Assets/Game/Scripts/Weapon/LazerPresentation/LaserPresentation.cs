using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class LaserPresentation : MonoBehaviour
    {
        private float _timer;

        public Laser LaserBody { get; private set; }
        public event System.Action<Laser, PhysicsVisual> OnLaserCollided;

        private ParticleHitEffectPool _poolHitEffect;

        [Inject]
        public void Construct(ParticleHitEffectPool poolHitEffect)
        {
            _poolHitEffect = poolHitEffect;
        }

        public void Init(Laser body, float lifetime)
        {
            LaserBody = body;
            _timer = lifetime;
            gameObject.SetActive(true);
        }

        private void Update()
        {
            if (!gameObject.activeSelf)
                return;

            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                Deactivate();
            }
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
            LaserBody = null;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (LaserBody == null)
                return;

            if (other.TryGetComponent(out AsteroidPresentation asteroid))
            {
                _poolHitEffect.Get(asteroid.transform.position);
                OnLaserCollided?.Invoke(LaserBody, asteroid);
            }

            if (other.TryGetComponent(out UfoPresentation ufo))
            {
                _poolHitEffect.Get(ufo.transform.position);
                OnLaserCollided?.Invoke(LaserBody, ufo);
            }
        }
    }
}