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
        
        private void Update()
        {
            // Проверяем, что астероид активен и зарегистрирован в физическом мире
            // if (AsteroidBody == null || _world == null || !gameObject.activeSelf)
            //     return;
                
            // Убираем дублирующий wrap-around, так как он уже происходит в PhysicsWorld.FixedTick()
            // Здесь только синхронизируем визуальное представление
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

                // Создаем новые астероиды перед деактивацией текущего
                int count = Random.Range(_config.minCountAsteroidSpawn, _config.maxCountAsteroidSpawn + 1);
                float newSpeed = AsteroidBody.Speed * 1.5f;
                Vector2 position = AsteroidBody.Position;

                for (int i = 0; i < count; i++)
                {
                    // Добавляем небольшое случайное смещение для каждого астероида
                    Vector2 offset = Random.insideUnitCircle * 2f;
                    Vector2 spawnPosition = position + offset;
                    factory.SpawnAsteroidAt(spawnPosition, newSpeed, newType);
                }
            }

            // Деактивируем текущий астероид после создания новых
            pool.Release(this);
        }

        private AsteroidType GetAsteroidType()
        {
            AsteroidType newType = _asteroidType switch
            {
                AsteroidType.Large => AsteroidType.Medium,
                AsteroidType.Medium => AsteroidType.Small,
                AsteroidType.Small => AsteroidType.Small, // Маленькие астероиды не разламываются
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
            _world.Register(this); // Register проверяет дубликаты
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            if (_world != null)
                _world.Unregister(this);
            
            // Очищаем состояние астероида
            AsteroidBody = null;
            gameObject.SetActive(false);
        }
    }
}