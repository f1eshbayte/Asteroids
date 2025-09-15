using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class GameController : MonoBehaviour
    {
        private AsteroidFactory _asteroidFactory;
        private ShipPresentation _shipPresentation;
        private AsteroidPool _asteroidPool;
        
        // private UfoPresentation _ufoPresentation; // сделать так же с тарелками коллизии
        private UfoPool _ufoPool;   // сделать так же с тарелками коллизии
        

        [Inject]
        public void Construct(AsteroidFactory asteroidFactory, ShipPresentation shipPresentation, AsteroidPool asteroidPool,
            /*UfoPresentation ufoPresentation,*/ UfoPool ufoPool)
        {
            _asteroidFactory = asteroidFactory;
            _shipPresentation = shipPresentation;
            _asteroidPool = asteroidPool;

            // _ufoPresentation = ufoPresentation;
            _ufoPool = ufoPool;
            
            // Подписываемся на событие столкновения в Construct, а не в Awake
            _shipPresentation.OnShipCollided += HandleShipCollision;
        }

        private void HandleShipCollision(Ship ship, PhysicsVisual target)
        {
            CollisionResolver.Resolve(ship, target, _asteroidFactory, _asteroidPool, _ufoPool);
            Debug.Log("Ship collision handled!");
        }


        private void OnDestroy()
        {
            _shipPresentation.OnShipCollided -= HandleShipCollision;
        }
    }
}