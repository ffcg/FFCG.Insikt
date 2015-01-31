using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNet.SignalR.Hubs;

namespace FFCG.Brun7
{
    public class BingoGame
    {
        private int _rows = 10;
        private Stack<int> _randomNumbers;
        private bool _isStarted;
        private Timer _timer;

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
            player.AddCard(new Card(_rows));
            Players.Add(player);
        }

        public void StartGame(IHubConnectionContext<dynamic> players)
        {
            _isStarted = true;

            var numbers = Enumerable.Range(1, _rows * 5).OrderBy(x => Guid.NewGuid());
            _randomNumbers = new Stack<int>(numbers);

            _timer = new Timer(DrawNumber, players, 0, 500);

            
        }

        public void Reset()
        {
            foreach (var bingoPlayer in Players)
            {
                bingoPlayer.AddCard(new Card(10));
            }
        }

        private void DrawNumber(dynamic players)
        {
            if (!_isStarted)
                throw new Exception("Game is not started!");

            if (!_randomNumbers.Any())
            {
                return;
            }

            var currentNumber = _randomNumbers.Pop();

            Players.ForEach(x => x.Card.Check(currentNumber));

            var playerWithBingo = Players.FirstOrDefault(x => x.Card.IsBingo());

            players.Group(RoomId).refreshCurrentGameState(currentNumber, Players);
            
            if (playerWithBingo != null)
            {
                players.Group(RoomId).announceBingoWinner(playerWithBingo.Name);
                StopGame();
            }
            
        }

        private void StopGame()
        {
            _timer.Dispose();
        }
    }
}