using System.Collections.Generic;
using Destroyer.TwoD;

namespace Destroyer.Game
{
    public class Board
    {
        public Board()
        {
            Obstacles = new List<Obstacle>();
            AllItems = new List<Item>();
        }

        public int Level;
        public Rect Size;
        public List<Obstacle> Obstacles;
        public List<Item> AllItems;
    }
}