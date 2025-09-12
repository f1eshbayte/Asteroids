using UnityEngine;

namespace Asteroids
{
    public class ShipModel : Body2D
    {
        public float Rotation; // угол в градусах
        public float AccelerationPower;
        public float RotationSpeed;
        public float Drag; // сопротивление (трение космоса)

        public ShipModel(Vector2 position, float radius, float mass, float accelerationPower, float rotationSpeed,
            float drag) : base(position, radius, mass)
        {
            Rotation = 0f;
            AccelerationPower = accelerationPower;
            RotationSpeed = rotationSpeed;
            Drag = drag;
        }

        public void Rotate(float input, float deltaTime)
        {
            Rotation -= input * RotationSpeed * deltaTime;
        }

        public void Thrust(float input, float deltaTime)
        {
            // переводим угол в вектор направления
            Vector2 dir = new Vector2(Mathf.Cos(Rotation * Mathf.Deg2Rad),
                Mathf.Sin(Rotation * Mathf.Deg2Rad));

            Velocity += dir * (input * AccelerationPower * deltaTime);
        }

        public void ApplyDrag(float deltaTime)
        {
            Velocity *= (1 - Drag * deltaTime);
        }
    }
}