namespace Destroyer.Game
{
    public class StaticTimer : ITimer
    {
        public int _ticks;

        public int Ticks()
        {
            return _ticks;
        }
        public float Elapsed()
        {
            return 1.0f / 60.0f;
        }

        public void Start()
        {

        }

        public void Stop()
        {

        }

        public void Update()
        {
            _ticks++;
        }
    }
}