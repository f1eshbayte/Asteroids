using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class PhysicsWorld : ITickable, IFixedTickable
    {
        private readonly List<Body2D> _bodies = new();
        private readonly List<PhysicsView> _views = new();

        // публичные настройки карты (можно менять в инспекторе через Installer)
        public float Width { get; private set; } = 1000f;
        public float Height { get; private set; } = 1000f;

        private float HalfWidth => Width / 2f;
        private float HalfHeight => Height / 2f;

        // Метод для установки размеров карты
        public void SetWorldSize(float width, float height)
        {
            Width = width;
            Height = height;
        }

        public void Register(PhysicsView view)
        {
            _views.Add(view);
            _bodies.Add(view.Body);
        }

        public void FixedTick()
        {
            float fixedDeltaTime = Time.fixedDeltaTime;

            foreach (var body in _bodies)
            {
                body.Integrate(fixedDeltaTime);

                // wrap-around по X
                if (body.Position.x > HalfWidth) 
                    body.Position.x = -HalfWidth;
                else if (body.Position.x < -HalfWidth) 
                    body.Position.x = HalfWidth;

                // wrap-around по Y
                if (body.Position.y > HalfHeight) 
                    body.Position.y = -HalfHeight;
                else if (body.Position.y < -HalfHeight) 
                    body.Position.y = HalfHeight;
            }

            // TODO: столкновения
        }

        public void Tick()
        {
            foreach (var view in _views)
            {
                view.SyncTransform();
            }
        }
    }
}
