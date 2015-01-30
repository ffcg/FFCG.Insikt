using System;
using System.Collections.Generic;

namespace FFCG.Brun7
{
    public class BingoGame
    {
        public Guid Id { get; private set; }
        public string RoomId { get; private set; }
        public List<BingoPlayer> Players { get; private set; } 

        public BingoGame(string roomId)
        {
            RoomId = roomId;
            Id = Guid.NewGuid();
            Players = new List<BingoPlayer>();
        }

        public void AddPlayer(BingoPlayer player)
        {
            player.AddCard(new Card(10));
            Players.Add(player);
        }
    }
}