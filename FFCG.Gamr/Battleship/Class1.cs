using System;
using System.Collections.Generic;

namespace Battleship
{
    public class Game
    {
        public List<Player> Players { get; set; }
        public int NumberOfShipsPerPlayer { get; set; }
        public DateTime Started { get; set; }
        public Guid ActivePlayer { get; set; }
    }

    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Board Board { get; set; }
    }

    public class Board
    {
        public List<Ship> Ships { get; private set; }
        public List<ShotsFired> ShotsFired { get; private set; }
    }

    public class Ship
    {
        public Guid Id { get; set; }
        public Cell Cell { get; set; }
        public bool IsFloating { get; set; }
    }

    public class ShotsFired
    {
        public Cell Cell { get; set; }
        public bool Hit { get; set; }
    }

    public class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}