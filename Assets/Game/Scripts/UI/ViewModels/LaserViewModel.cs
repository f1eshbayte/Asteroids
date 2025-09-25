using System;
using MVVM;
using UniRx;
using Zenject;

namespace Asteroids
{
    public class LaserViewModel : IInitializable, IDisposable
    {
        [Data("CurrentShots")] public ReactiveProperty<string> CurrentShots = new();
        [Data("RechargeTimer")] public ReactiveProperty<string> RechargeTimer = new();

        private PlayerWeapons _weapon;

        public LaserViewModel(PlayerWeapons weapon)
        {
            _weapon = weapon;
        }

        public void Initialize()
        {
            OnShotsChanged(_weapon.CurrentLaserShots);
            OnChargeRecovery(_weapon.RechargeTimer);
            _weapon.OnShootLaser += OnShotsChanged;
            _weapon.OnChargeRecovery += OnChargeRecovery;
        }

        public void Dispose()
        {
            _weapon.OnShootLaser -= OnShotsChanged;
            _weapon.OnChargeRecovery -= OnChargeRecovery;
        }
        
        private void OnChargeRecovery(float time)
        {
            RechargeTimer.Value = $"Recovery:{time:F2}s."; 
        }

        private void OnShotsChanged(int shots)
        {
            CurrentShots.Value = $"Shots: {shots}"; 
        }
    }
}