using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleship.Domain
{
    public class Player
    {
        private readonly int _numberOfShips;

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public List<Ship> Ships { get; private set; }
        public List<ShotsFired> ShotsFired { get; private set; }

        public bool AllShipsAreSunk
        {
            get { return Ships.All(x => !x.IsFloating); }
        }

        public Player(string name, int numberOfShips)
        {
            _numberOfShips = numberOfShips;
            Id = Guid.NewGuid();
            Name = name;
            Ships = new List<Ship>();
            ShotsFired = new List<ShotsFired>();
        }

        public Ship PlaceShip(int x, int y)
        {
            if (Ships.Count() == _numberOfShips)
                throw new ApplicationException("Player has placed all available ships");

            var location = new Cell(x, y);

            if (Ships.Any(s => s.Location.Equals(location)))
                throw new ApplicationException("Cell is already occupied");

            var ship = new Ship(location);
            Ships.Add(ship);
            
            return ship;
        }

        public void LogShotAtEnemy(Cell target, bool isHit)
        {
            ShotsFired.Add(new ShotsFired(target, isHit));
        }

        public bool ShootAt(Cell target)
        {
            var ship = Ships.FirstOrDefault(x => x.Location.Equals(target));

            if (ship != null)
            {
                ship.Sink();
                return true;
            }

            return false;
        }
    }
}