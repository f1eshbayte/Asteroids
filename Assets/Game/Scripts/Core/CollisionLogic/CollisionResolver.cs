using UnityEngine;

namespace Asteroids
{
    public static class CollisionResolver
    {
        public static void Resolve(Ship shipBody, PhysicsVisual target, AsteroidFactory asteroidFactory, 
            AsteroidPool asteroidPool, UfoPool ufoPool)
        {
            switch (target)
            {
                case AsteroidPresentation asteroid:
                    Resolve(shipBody, asteroid, asteroidFactory, asteroidPool);
                    break;

                case UfoPresentation ufo:
                    Resolve(shipBody, ufo, ufoPool);
                    break;

                default:
                    Debug.LogWarning($"No resolver for {target.GetType().Name}");
                    break;
            }
        }
        
        public static void Resolve(Bullet bulletBody, PhysicsVisual target, 
            AsteroidFactory asteroidFactory, AsteroidPool asteroidPool, UfoPool ufoPool)
        {
            switch (target)
            {
                case AsteroidPresentation asteroid:
                    Resolve(bulletBody, asteroid, asteroidFactory, asteroidPool);
                    break;

                case UfoPresentation ufo:
                    Resolve(bulletBody, ufo, ufoPool);
                    break;

                default:
                    Debug.LogWarning($"No resolver for {target.GetType().Name}");
                    break;
            }
        }
        public static void Resolve(Laser laserBody, PhysicsVisual target, 
            AsteroidFactory asteroidFactory, AsteroidPool asteroidPool, UfoPool ufoPool)
        {
            switch (target)
            {
                case AsteroidPresentation asteroid:
                    Resolve(laserBody, asteroid);
                    break;

                case UfoPresentation ufo:
                    Resolve(laserBody, ufo, ufoPool);
                    break;

                default:
                    Debug.LogWarning($"No resolver for {target.GetType().Name}");
                    break;
            }
        }

        private static void Resolve(Ship shipBody, AsteroidPresentation asteroid,
            AsteroidFactory factory, AsteroidPool pool)
        {
            PhysicsEngine.ResolveCollision(shipBody, asteroid.AsteroidBody);
            asteroid.Split(factory, pool);
        }

        private static void Resolve(Ship shipBody, UfoPresentation ufo, UfoPool pool)
        {
            PhysicsEngine.ResolveCollision(shipBody, ufo.UfoBody);
            ufo.OnHit(pool);
        }
        
        private static void Resolve(Bullet bulletBody, UfoPresentation ufo, UfoPool pool)
        {
            PhysicsEngine.ResolveCollision(bulletBody, ufo.UfoBody);
            ufo.OnHit(pool);
        }
        
        private static void Resolve(Bullet bulletBody, AsteroidPresentation asteroid,
            AsteroidFactory factory, AsteroidPool pool)
        {
            PhysicsEngine.ResolveCollision(bulletBody, asteroid.AsteroidBody);
            asteroid.Split(factory, pool);
        }
        
        private static void Resolve(Laser laserBody, UfoPresentation ufo, UfoPool pool)
        {
            PhysicsEngine.ResolveCollision(laserBody, ufo.UfoBody);
            ufo.OnHit(pool);
        }
        
        private static void Resolve(Laser laserBody, AsteroidPresentation asteroid)
        {
            PhysicsEngine.ResolveCollision(laserBody, asteroid.AsteroidBody);
            asteroid.Deactivate();
        }
    }
}