using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Asteroids
{
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class AsteroidPresentation : PhysicsVisual
    {
        [SerializeField] private bool _isShared;
        [SerializeField] private AsteroidType _asteroidType;
        [SerializeField] private EnemyType _enemyType;
        private AsteroidsConfig _config;
        private PhysicsWorld _world;
        public Asteroid AsteroidBody { get; private set; }
        public AsteroidType AsteroidType => _asteroidType;

        [Inject]
        public void Construct(PhysicsWorld world, AsteroidsConfig config)
        {
            _world = world;
            _config = config;
        }

        public void Split(AsteroidFactory factory, AsteroidPool pool)
        {
            if (_isShared && AsteroidBody != null)
            {
                var newType = GetAsteroidType();

                if (_asteroidType == AsteroidType.Small)
                {
                    pool.Release(this);
                    return;
                }

                int count = Random.Range(_config.minCountAsteroidSpawn, _config.maxCountAsteroidSpawn + 1);
                float newSpeed = AsteroidBody.Speed * 1.5f;
                Vector2 position = AsteroidBody.Position;

                for (int i = 0; i < count; i++)
                {
                    Vector2 offset = Random.insideUnitCircle * 2f;
                    Vector2 spawnPosition = position + offset;
                    factory.SpawnAsteroidAt(spawnPosition, newSpeed, newType);
                }
            }

            pool.Release(this);
        }

        private AsteroidType GetAsteroidType()
        {
            AsteroidType newType = _asteroidType switch
            {
                AsteroidType.Large => AsteroidType.Medium,
                AsteroidType.Medium => AsteroidType.Small,
                AsteroidType.Small => AsteroidType.Small, 
                _ => AsteroidType.Small
            };
            return newType;
        }

        public void Activate(Vector2 position, float speed)
        {
            if (AsteroidBody == null)
                AsteroidBody = new Asteroid(position, _config.mass, speed);
            else
                AsteroidBody.Reset(position, speed);

            Init(AsteroidBody, _enemyType);
            _world.Register(this); 
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            if (_world != null)
                _world.Unregister(this);
            
            AsteroidBody = null;
            gameObject.SetActive(false);
        }
    }
}