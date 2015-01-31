using System;

namespace Destroyer.Game
{
    public class Timer : ITimer
    {
        private int _elapsedTicks;
        public int _ticks;

        public int Ticks()
        {
            return _ticks;
        }

        public float Elapsed()
        {
            return _elapsedTicks / 1000.0f;
        }

        public void Start()
        {
            _elapsedTicks = Environment.TickCount;
            _ticks = 0;
        }

        public void Stop()
        {
            _ticks = 0;
        }

        public void Update()
        {
            _ticks++;
            _elapsedTicks = (int)Math.Min(Environment.TickCount - _elapsedTicks, 1000/10);
        }
    }
}