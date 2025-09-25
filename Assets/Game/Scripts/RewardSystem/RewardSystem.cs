using System;
using System.Collections.Generic;

namespace Asteroids
{
    public class RewardSystem
    {
        private Dictionary<EnemyType, int> _rewards = new()
        {
            { EnemyType.Asteroid, 100 },
            { EnemyType.Ufo, 200 }
        };

        public event Action<int> OnStateChanged;
        
        public int TotalScore { get; private set; }

        public void AddScore(EnemyType type)
        {
            if (_rewards.TryGetValue(type, out int reward))
            {
                TotalScore += reward;
                OnStateChanged?.Invoke(TotalScore);
            }
        }
    }
}