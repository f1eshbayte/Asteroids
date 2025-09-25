namespace Asteroids
{
    public class PauseChangedSignal
    {
        public bool IsPaused;

        public PauseChangedSignal(bool isPaused)
        {
            IsPaused = isPaused;
        }
    }
}