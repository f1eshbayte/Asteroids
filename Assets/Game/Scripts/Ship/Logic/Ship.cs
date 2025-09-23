using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Asteroids
{
    public class Ship : Body2D
    {
        public float Rotation { get; private set; } 
        public float AccelerationPower { get; private set; }
        public float RotationSpeed { get; private set; }
        public float Drag { get; private set; } 
        public bool IsGod { get; private set; } = false;
        
        public Ship(Vector2 position, float mass, float accelerationPower, float rotationSpeed, float drag) : base(
            position, mass)
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
            Vector2 dir = new Vector2(Mathf.Cos(Rotation * Mathf.Deg2Rad), Mathf.Sin(Rotation * Mathf.Deg2Rad));

            Velocity += dir * (input * AccelerationPower * deltaTime);
        }

        public void ApplyDrag(float deltaTime)
        {
            Velocity *= (1 - Drag * deltaTime);
        }

        public void Respawn(Vector2 position, float godDuratioin)
        {
            Position = position;
            Velocity = Vector2.zero;
            IsGod = true;
            ExpirationTime(godDuratioin).Forget();
        }

        private async UniTask ExpirationTime(float duration)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(duration));

            IsGod = false;
        }
    }
}