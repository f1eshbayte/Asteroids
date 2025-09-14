using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class PhysicsWorld : ITickable, IFixedTickable
    {
        private readonly List<Body2D> _bodies = new();
        private readonly List<PhysicsVisual> _physicsVisuals = new();

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

        public void Register(PhysicsVisual visual)
        {
            // Проверяем, не зарегистрирован ли уже объект
            if (!_physicsVisuals.Contains(visual))
            {
                _physicsVisuals.Add(visual);
                if (visual.Body != null && !_bodies.Contains(visual.Body))
                {
                    _bodies.Add(visual.Body);
                }
            }
        }

        public void Unregister(PhysicsVisual visual)
        {
            _physicsVisuals.Remove(visual);
            if (visual.Body != null)
                _bodies.Remove(visual.Body);
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
            foreach (var visual in _physicsVisuals)
            {
                visual.SyncTransform();
            }
        }
    }
}
