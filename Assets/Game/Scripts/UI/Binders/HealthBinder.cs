using System;
using MVVM;
using UniRx;

namespace Asteroids
{
    public class HealthBinder : IBinder, IObserver<int>
    {
        private readonly ReactiveProperty<int> _viewProperty;
        private readonly IReadOnlyReactiveProperty<int> _modelProperty;
        private IDisposable _handle;

        public HealthBinder(ReactiveProperty<int> view, IReadOnlyReactiveProperty<int> property)
        {
            _viewProperty = view;
            _modelProperty = property;
        }

        public void Bind()
        {
            OnNext(_modelProperty.Value);
            _handle = _modelProperty.Subscribe(this);
        }

        public void Unbind()
        {
            _handle?.Dispose();
            _handle = null;
        }

        public void OnNext(int value)
        {
            _viewProperty.Value = value;
        }

        public void OnCompleted() { }
        public void OnError(Exception error) { }
    }
}