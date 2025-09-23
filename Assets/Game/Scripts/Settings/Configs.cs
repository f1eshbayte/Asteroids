using System;

namespace Asteroids
{
    [Serializable]
    public class ShipConfig
    {
        public int maxHealth;
        public int freezeTime;
        public int godDuration;
        public float mass;
        public float accelerationPower;
        public float rotationSpeed;
        public float drag;
        public float bulletSpeed;
        public int maxLaserShots;
        public float laserLifetime;
        public float laserRechargeTime;
    }

    [Serializable]
    public class AsteroidsConfig
    {
        public float mass;
        public int minCountAsteroidSpawn;
        public int maxCountAsteroidSpawn;
    }

    [Serializable]
    public class UfoConfig
    {
        public float mass;
        public float steerStrength;
        public float maxChaseDistance;
    }

    [Serializable]
    public class WorldMapConfig
    {
        public float worldWidth;
        public float worldHeight;
    }
}