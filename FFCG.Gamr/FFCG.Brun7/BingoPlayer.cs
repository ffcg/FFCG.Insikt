using System.Collections;
using System.Collections.Generic;

namespace FFCG.Brun7
{
    public class BingoPlayer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Card Card { get; private set; }

        public IEnumerable<Row> Rows { get { return Card.GetAllRows(); } }

        public BingoPlayer(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public void AddCard(Card card)
        {
            Card = card;
        }
    }
}