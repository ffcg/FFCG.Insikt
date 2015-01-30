using System.Collections.Generic;
using System.Linq;

namespace FFCG.Brun7
{
    public class Row
    {
        private readonly List<Square> _squares;
        public IEnumerable<Square> Squares { get { return _squares; } }

        public Row()
        {
            _squares = new List<Square>();
        }

        public void AddSquare(Square square)
        {
            _squares.Add(square);
        }

        public bool HasBingo()
        {
            return _squares.All(x => x.Checked);
        }
    }
}