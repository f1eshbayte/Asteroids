using UnityEngine;

namespace Asteroids
{
    // public static class CollisionResolver
    // {
    //     
    //     
    //     
    //     public static void Resolve(Ship ship, AsteroidPresentation asteroid, AsteroidFactory factory, AsteroidPool pool)
    //     {
    //         PhysicsEngine.ResolveCollision(ship, asteroid.AsteroidBody);
    //     
    //         asteroid.Split(factory, pool); // фабрика передаётся явно
    //         // ship.Respawn(Vector2.zero, 3f);
    //     }
    //
    //     public static void Resolve(Ship ship, UfoPresentation ufo, UfoPool pool)
    //     {
    //         PhysicsEngine.ResolveCollision(ship, ufo.UfoBody);
    //         
    //         ufo.OnHit(pool);
    //     }
    //     
    // }
    
    public static class CollisionResolver
    {
        public static void Resolve(Ship ship, PhysicsVisual target, AsteroidFactory asteroidFactory, 
            AsteroidPool asteroidPool, UfoPool ufoPool)
        {
            switch (target)
            {
                case AsteroidPresentation asteroid:
                    Resolve(ship, asteroid, asteroidFactory, asteroidPool);
                    break;

                case UfoPresentation ufo:
                    Resolve(ship, ufo, ufoPool);
                    break;

                default:
                    Debug.LogWarning($"No resolver for {target.GetType().Name}");
                    break;
            }
        }

        private static void Resolve(Ship ship, AsteroidPresentation asteroid,
            AsteroidFactory factory, AsteroidPool pool)
        {
            PhysicsEngine.ResolveCollision(ship, asteroid.AsteroidBody);
            asteroid.Split(factory, pool);
        }

        private static void Resolve(Ship ship, UfoPresentation ufo, UfoPool pool)
        {
            PhysicsEngine.ResolveCollision(ship, ufo.UfoBody);
            ufo.OnHit(pool);
        }
    }
}