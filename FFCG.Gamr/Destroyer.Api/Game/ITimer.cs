namespace Destroyer.Game
{
    public interface ITimer
    {
        int Ticks();
        float Elapsed();
        void Start();
        void Stop();
        void Update();
    }
}