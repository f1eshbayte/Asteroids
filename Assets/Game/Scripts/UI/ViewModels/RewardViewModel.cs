using System;
using MVVM;
using UniRx;
using Zenject;

namespace Asteroids
{
    public class RewardViewModel : IInitializable, IDisposable
    {
        [Data("Reward")]
        public ReactiveProperty<string> Reward = new();
        private readonly RewardSystem _rewardSystem;

        public RewardViewModel(RewardSystem rewardSystem)
        {
            _rewardSystem = rewardSystem;
        }
        public void Initialize()
        {
            OnRewardChanged(_rewardSystem.TotalScore);
            _rewardSystem.OnStateChanged += OnRewardChanged;
        }

        public void Dispose()
        {
            _rewardSystem.OnStateChanged -= OnRewardChanged;
        }

        private void OnRewardChanged(int reward)
        {
            Reward.Value = $"Score: {reward}";
        }
    }
}