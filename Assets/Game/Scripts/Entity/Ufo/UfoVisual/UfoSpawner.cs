using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;

namespace Asteroids
{
    public class UfoSpawner : MonoBehaviour
    {
        [SerializeField] private float _minInterval = 15f;
        [SerializeField] private float _maxInterval = 30f;
        [SerializeField] private int _maxActiveUfos = 1;
        [SerializeField] private float _ufoSpeed = 12f;

        private UfoFactory _factory;
        private UfoPool _pool;
        private Ship _ship;

        [Inject]
        public void Construct(UfoFactory factory, UfoPool pool, Ship ship)
        {
            _factory = factory;
            _pool = pool;
            _ship = ship;
        }

        private void Start()
        {
            SpawnLoop().Forget();
        }

        private async UniTask SpawnLoop()
        {
            while (true)
            {
                if (_pool.ActiveCount < _maxActiveUfos)
                {
                    _factory.SpawnAtEdge(_ufoSpeed, _ship);
                }

                float wait = Random.Range(_minInterval, _maxInterval);
                await UniTask.Delay(System.TimeSpan.FromSeconds(wait));
            }
        }
    }
}