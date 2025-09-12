using UnityEngine;
using Zenject;

namespace Asteroids
{
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class AsteroidView : PhysicsView
    {
        public AsteroidModel Model { get; private set; }
        private PhysicsWorld _world;

        [Inject]
        public void Construct(PhysicsWorld world)
        {
            _world = world;
        }

        public void Activate(Vector2 position, float speed)
        {
            if (Model == null)
                Model = new AsteroidModel(position, speed: speed);
            else
                Model.Reset(position, speed);

            Init(Model);
            _world.Register(this);
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Model == null || _world == null) return;

            float w = _world.Width / 2f;
            float h = _world.Height / 2f;

            // wrap-around
            Vector2 pos = Model.Position;

            if (pos.x > w)
                pos.x = -w;
            else if (pos.x < -w)
                pos.x = w;

            if (pos.y > h)
                pos.y = -h;
            else if (pos.y < -h)
                pos.y = h;

            Model.Position = pos;
        }
    }
}