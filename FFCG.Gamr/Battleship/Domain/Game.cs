using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleship.Domain
{
    public class Game
    {
        private Player _activePlayer;

        public Guid Id { get; private set; }
        public List<Player> Players { get; private set; }
        public int NumberOfShipsPerPlayer { get; private set; }
        public DateTime? Started { get; private set; }

        public bool IsStarted
        {
            get { return Started.HasValue; }
        }

        public bool IsWaitingForPlayers
        {
            get { return Players.Count() < 2; }
        }

        public bool AllShipsArePlaced
        {
            get { return Players.All(x => x.Ships.Count == NumberOfShipsPerPlayer); }
        }

        public Game(int numberOfShipsPerPlayer)
        {
            Id = Guid.NewGuid();
            Players = new List<Player>();
            NumberOfShipsPerPlayer = numberOfShipsPerPlayer;
        }

        public Player GetPlayer(Guid playerId)
        {
            return Players.First(x => x.Id == playerId);
        }

        public Player GetEnemyOf(Guid playerId)
        {
            return Players.First(x => x.Id != playerId);
        }

        public Player AddPlayer(string name)
        {
            if (!IsWaitingForPlayers)
            {
                throw new ApplicationException("No more players allowed");
            }

            var player = new Player(name, NumberOfShipsPerPlayer);
            Players.Add(player);
            return player;
        }

        public void Start()
        {
            if (IsStarted)
                throw new ApplicationException("Game has already started");

            if (IsWaitingForPlayers)
                throw new ApplicationException("Still waiting for players");

            Started = DateTime.Now;
        }

        public Player GetActivePlayer()
        {
            if (IsStarted)
                throw new ApplicationException("Game has not yet started");
            
            if (_activePlayer == null)
            {
                _activePlayer = Players.OrderBy(x => x.Id).First();
            }

            return _activePlayer;
        }

        public bool Fire(Guid playerId, Guid enemyPlayerId, Cell target)
        {
            var player = Players.Single(x => x.Id == playerId);
            var enemyPlayer = Players.Single(x => x.Id == enemyPlayerId);

            var sunkShip = enemyPlayer.ShootAt(target);

            player.LogShotAtEnemy(target, sunkShip);

            if (!sunkShip)
            {
                SwitchPlayer();
            }
            
            return sunkShip;
        }

        private void SwitchPlayer()
        {
            _activePlayer = Players.First(x => x.Id != _activePlayer.Id);
        }
    }
}