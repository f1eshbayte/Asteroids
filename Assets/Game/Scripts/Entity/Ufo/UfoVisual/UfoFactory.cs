using UnityEngine;

namespace Asteroids
{
    public class UfoFactory
    {
        private readonly UfoPool _pool;
        private readonly PhysicsWorld _world;

        public UfoFactory(UfoPool pool, PhysicsWorld world)
        {
            _pool = pool;
            _world = world;
        }

        public void SpawnAtEdge(float speed, Ship target)
        {
            float halfW = _world.Width / 2f;
            float halfH = _world.Height / 2f;

            float x = Random.value > 0.5f ? halfW + 12f : -halfW - 12f;
            float y = Random.Range(-halfH, halfH);

            Vector2 pos = new Vector2(x, y);
            _pool.Get(pos, speed, target);
        }

        public void SpawnAt(Vector2 position, float speed, Ship target)
        {
            _pool.Get(position, speed, target);
        }
    }
}