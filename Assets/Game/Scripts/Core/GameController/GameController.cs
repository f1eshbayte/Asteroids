using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class GameController : MonoBehaviour
    {
        private AsteroidFactory _asteroidFactory;
        private ShipPresentation _shipPresentation;
        private AsteroidPool _pool;

        [Inject]
        public void Construct(AsteroidFactory asteroidFactory, ShipPresentation shipPresentation, AsteroidPool pool)
        {
            _asteroidFactory = asteroidFactory;
            _shipPresentation = shipPresentation;
            _pool = pool;
            
            // Подписываемся на событие столкновения в Construct, а не в Awake
            _shipPresentation.OnShipCollided += HandleShipCollision;
        }

        private void HandleShipCollision(Ship ship, AsteroidPresentation asteroid)
        {
            CollisionResolver.Resolve(ship, asteroid, _asteroidFactory, _pool);
            Debug.Log("Ship collision handled!");
        }


        private void OnDestroy()
        {
            _shipPresentation.OnShipCollided -= HandleShipCollision;
        }
    }
}