namespace Battleship.Domain
{
    public class ShotsFired
    {
        public ShotsFired(Cell cell, bool hit)
        {
            Hit = hit;
            Cell = cell;
        }

        public Cell Cell { get; private set; }
        public bool Hit { get; private set; }
    }
}