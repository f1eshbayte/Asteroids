using System;
using MVVM;
using UniRx;
using Zenject;

namespace Asteroids
{
    public class ShipCoordinatesViewModel : IInitializable, IDisposable
    {
        [Data("Position")] public ReactiveProperty<string> Position = new();
        [Data("Rotation")] public ReactiveProperty<string> Rotation = new();
        [Data("Velocity")] public ReactiveProperty<string> Velocity = new();

        private readonly ShipPresentation _ship;


        public ShipCoordinatesViewModel(ShipPresentation ship)
        {
            _ship = ship;
        }

        public void Initialize()
        {
            UpdateProperties();
            _ship.OnUpdated += UpdateProperties;
        }


        public void Dispose()
        {
            _ship.OnUpdated -= UpdateProperties;
        }
        private void UpdateProperties()
        {
            var pos = _ship.transform.position;
            Position.Value = $"X: {pos.x:F2}, Y: {pos.y:F2}";
            Rotation.Value = $"{_ship.transform.eulerAngles.z:F1}°";
            Velocity.Value = $"{_ship.ShipBody.Velocity.magnitude:F2}";
        }
    }
}