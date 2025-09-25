using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Asteroids
{
    public class AsteroidSpawner : MonoBehaviour
    {
        [SerializeField] private float _spawnInterval = 2f;
        [SerializeField] private float _minSpeed = 10f;
        [SerializeField] private float _maxSpeed = 20f;
        [SerializeField] private int _maxActiveAsteroids = 10; 

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
        
        private void OnEnable()
        {
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private void OnSceneUnloaded(Scene scene)
        {
            _pool.Clear(); 
        }

        private async UniTask SpawnLoop()
        {
            while (this != null && gameObject != null) 
            {
                if (_pool != null && _pool.ActiveCount < _maxActiveAsteroids)
                {
                    float speed = Random.Range(_minSpeed, _maxSpeed);
                    _factory.SpawnAsteroid(speed);
                }

                await UniTask.Delay(System.TimeSpan.FromSeconds(_spawnInterval));

                if (this == null || gameObject == null) 
                    break;
            }
        }

    }
}
