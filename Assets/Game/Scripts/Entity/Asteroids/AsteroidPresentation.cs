using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Asteroids
{
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class AsteroidPresentation : PhysicsVisual
    {
        [SerializeField] private float _mass = 1;
        [SerializeField] private bool _isShared;
        [SerializeField] private int _minCountAsteroidSpawn = 2;
        [SerializeField] private int _maxCountAsteroidSpawn = 4;
        [SerializeField] private AsteroidType _type;

        private PhysicsWorld _world;
        public Asteroid Asteroid { get; private set; }
        public AsteroidType Type => _type;

        [Inject]
        public void Construct(PhysicsWorld world)
        {
            _world = world;
        }
        
        private void Update()
        {
            // Проверяем, что астероид активен и зарегистрирован в физическом мире
            if (Asteroid == null || _world == null || !gameObject.activeSelf)
                return;
                
            // Убираем дублирующий wrap-around, так как он уже происходит в PhysicsWorld.FixedTick()
            // Здесь только синхронизируем визуальное представление
        }

        public void Split(AsteroidFactory factory, AsteroidPool pool)
        {
            if (_isShared && Asteroid != null)
            {
                // Определяем тип новых астероидов в зависимости от текущего типа
                var newType = GetAsteroidType();

                // Маленькие астероиды просто умирают (не разламываются)
                if (_type == AsteroidType.Small)
                {
                    pool.Release(this);
                    return;
                }

                // Создаем новые астероиды перед деактивацией текущего
                int count = Random.Range(_minCountAsteroidSpawn, _maxCountAsteroidSpawn + 1);
                float newSpeed = Asteroid.Speed * 1.5f;
                Vector2 position = Asteroid.Position;

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
            AsteroidType newType = _type switch
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
            if (Asteroid == null)
                Asteroid = new Asteroid(position, _mass, speed);
            else
                Asteroid.Reset(position, speed);

            Init(Asteroid);
            _world.Register(this); // Register проверяет дубликаты
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            if (_world != null)
                _world.Unregister(this);
            
            // Очищаем состояние астероида
            Asteroid = null;
            gameObject.SetActive(false);
        }
    }
}