using UnityEngine;

namespace Asteroids
{
    public static class CollisionResolver
    {
        public static void Resolve(Ship ship, AsteroidPresentation asteroid, AsteroidFactory factory, AsteroidPool pool)
        {
            PhysicsEngine.ResolveCollision(ship, asteroid.Asteroid);
        
            asteroid.Split(factory, pool); // фабрика передаётся явно
            // ship.Respawn(Vector2.zero, 3f);
        }
    }
}