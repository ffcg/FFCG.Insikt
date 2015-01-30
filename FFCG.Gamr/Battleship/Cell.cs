using System;

namespace Battleship
{
    public class Cell : IEquatable<Cell>
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Cell other)
        {
            if (other == null)
                return false;

            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Cell);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }
    }
}