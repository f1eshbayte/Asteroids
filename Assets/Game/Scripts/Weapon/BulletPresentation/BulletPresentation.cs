using System;
using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class BulletPresentation : PhysicsVisual
    {
        public Bullet BulletBody { get; private set; }
        public PhysicsWorld _world;

        public event Action<Bullet, PhysicsVisual> OnBulletCollided;

        private ParticleHitEffectPool _poolHitEffect;
        private EnemyType _type = EnemyType.None;

        [Inject]
        public void Construct(ParticleHitEffectPool poolHitEffect)
        {
            _poolHitEffect = poolHitEffect;
        }

        public void InitBullet(Bullet body, PhysicsWorld world)
        {
            BulletBody = body;
            _world = world;
            Init(body, _type);
            _world.Register(this);
            gameObject.SetActive(true);
        }

        private void Update()
        {
            if (BulletBody == null || !gameObject.activeSelf)
                return;

            if (!BulletBody.UpdateLifetime(Time.deltaTime))
            {
                gameObject.SetActive(false);
                _world?.Unregister(this);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (BulletBody == null)
                return;

            if (other.TryGetComponent(out AsteroidPresentation asteroid))
            {
                _poolHitEffect.Get(asteroid.transform.position);
                OnBulletCollided?.Invoke(BulletBody, asteroid);
                gameObject.SetActive(false);
                _world?.Unregister(this);
            }

            if (other.TryGetComponent(out UfoPresentation ufo))
            {
                _poolHitEffect.Get(ufo.transform.position);
                OnBulletCollided?.Invoke(BulletBody, ufo);
                gameObject.SetActive(false);
                _world?.Unregister(this);
            }
        }
    }
}