namespace Asteroids
{
    public interface IPausable
    {
        void OnPauseChanged(PauseChangedSignal signal);
    }
}