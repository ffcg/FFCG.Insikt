using System;

namespace Battleship.Domain
{
    public class Ship
    {
        public Guid Id { get; private set; }
        public Cell Location { get; private set; }
        public bool IsFloating { get; private set; }
        public DateTime? SunkAt { get; private set; }

        public Ship(Cell location)
        {
            Id = Guid.NewGuid();
            Location = location;
        }

        public void Sink()
        {
            IsFloating = false;
            SunkAt = DateTime.Now;
        }
    }
}