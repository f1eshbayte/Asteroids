using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;

namespace Asteroids
{
    public class AsteroidSpawner : MonoBehaviour
    {
        [SerializeField] private float _spawnInterval = 2f;
        [SerializeField] private float _minSpeed = 20f;
        [SerializeField] private float _maxSpeed = 50f;
        [SerializeField] private int _maxActiveAsteroids = 10; // лимит активных астероидов

        private AsteroidFactory _factory;
        private AsteroidPool _pool;

        [Inject]
        public void Construct(AsteroidFactory factory, AsteroidPool pool)
        {
            _factory = factory;
            _pool = pool;
        }

        private void Start()
        {
            SpawnLoop().Forget();
        }

        private async UniTaskVoid SpawnLoop()
        {
            while (true)
            {
                if (_pool.ActiveCount < _maxActiveAsteroids)
                {
                    float speed = Random.Range(_minSpeed, _maxSpeed);
                    _factory.SpawnAsteroid(speed);
                }

                await UniTask.Delay(System.TimeSpan.FromSeconds(_spawnInterval));
            }
        }
    }
}
