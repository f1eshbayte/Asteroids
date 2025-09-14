using UnityEngine;

namespace Asteroids
{
    public class AsteroidFactory
    {
        private readonly AsteroidPool _pool;
        private readonly PhysicsWorld _world;

        public AsteroidFactory(AsteroidPool pool, PhysicsWorld world)
        {
            _pool = pool;
            _world = world;
        }

        public void SpawnAsteroid(float speed)
        {
            float halfW = _world.Width / 2f;
            float halfH = _world.Height / 2f;

            // спавн за пределами карты
            float x = Random.value > 0.5f ? halfW + 10f : -halfW - 10f;
            float y = Random.Range(-halfH, halfH);

            Vector2 pos = new Vector2(x, y);

            var asteroid = _pool.Get(pos, speed);
            
            if(asteroid == null)
                return;
        }

        public void SpawnAsteroidAt(Vector2 position, float speed, AsteroidType type)
        {
            _pool.Get(position, speed, type);
        }

    }
}