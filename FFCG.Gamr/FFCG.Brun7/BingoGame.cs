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
        public decimal Speed { get; private set; }

        private decimal _speed = 1;

        public BingoGame(string roomId)
        {
            RoomId = roomId;
            Id = Guid.NewGuid();
            Players = new List<BingoPlayer>();
            Speed = 1;
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

            _timer = new Timer(DrawNumber, players, 0, (int)(_speed * 1000));
        }

        public void IncreaseSpeed()
        {
            if (Speed == 1.9m)
                return;

            Speed += 0.1m;
            _speed -= 0.1m;
            if (_timer != null)
            {
                var period = (int)(_speed * 1000);
                _timer.Change(period, period);
            }
        }

        public void LowerSpeed()
        {
            if (Speed > 0)
            {
                Speed -= 0.1m;
                _speed += 0.1m;
                if(_timer != null)
                {
                    var period = (int)(_speed * 1000);
                    _timer.Change(period, period);
                }
            }
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

            var playerWithBingos = Players.Where(x => x.Card.IsBingo()).ToList();

            players.Group(RoomId).refreshCurrentGameState(currentNumber, this);
            
            if (playerWithBingos.Any())
            {
                var bingos = string.Join(" & ", playerWithBingos.Select(x => x.Name).ToArray());

                players.Group(RoomId).announceBingoWinner(bingos);
                StopGame();
            }
        }

        private void StopGame()
        {
            _timer.Dispose();
        }

        public void RemovePlayer(string connectionId)
        {
            Players.RemoveAll(x => x.Id == connectionId);
        }
    }
}