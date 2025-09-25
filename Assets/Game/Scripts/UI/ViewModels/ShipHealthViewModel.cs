using System;
using MVVM;
using UniRx;
using Zenject;

namespace Asteroids
{
    public class ShipHealthViewModel : IInitializable, IDisposable
    {
        [Data("Health")] public ReactiveProperty<int> Health = new();
        [Data("MaxHealth")] public ReactiveProperty<int> MaxHealth = new();

        private readonly ShipPresentation _ship;

        public ShipHealthViewModel(ShipPresentation ship)
        {
            _ship = ship;
        }

        public void Initialize()
        {
            MaxHealth.Value = _ship.MaxHealth;
            Health.Value = _ship.CurrentHealth; 
            _ship.OnHealthChanged += SyncHealth;
            _ship.OnUpdated += SyncAll;
        }

        public void Dispose()
        {
            _ship.OnHealthChanged -= SyncHealth;
            _ship.OnUpdated -= SyncAll;
        }

        private void SyncHealth()
        {
            Health.Value = _ship.CurrentHealth;
        }

        private void SyncAll()
        {
            MaxHealth.Value = _ship.MaxHealth;
            Health.Value = _ship.CurrentHealth;
        }
    }
}