using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Destroyer.Actions;
using Destroyer.Game;
using FFCG.Brun7;
using Microsoft.AspNet.SignalR;

namespace FFCG.Gamr.Destroyr
{
    public class DestroyrHub : Hub
    {
        public static List<DestroyrPlayer> Players; 

        public void CreateGame()
        {
            if (WebApiApplication.DestroyerGame == null)
            {
                WebApiApplication.DestroyerGame = GameBuilder.NewGame();
                Players = new List<DestroyrPlayer>();
                WebApiApplication.DestroyerGame.StartLevel(1);
            }
        }

        public void Join(string playerName)
        {
            var destroyrPlayer = new DestroyrPlayer() {ConnectionId = this.Context.ConnectionId, Name = playerName};
            var player = WebApiApplication.DestroyerGame.AddPlayer(playerName);
            destroyrPlayer.PlayerId = player.Id;

            Players.Add(destroyrPlayer);
        }

        public void UserAction(UserActionType action)
        {
            var game = WebApiApplication.DestroyerGame;
            var destroyrPlayer = Players.FirstOrDefault(p => p.ConnectionId == this.Context.ConnectionId);
            var player = game.Players.FirstOrDefault(p => p.Id == destroyrPlayer.PlayerId);

            switch (action)
            {
                case (UserActionType.Fire):
                    WebApiApplication.DestroyerGame.ShootProjectile(player);
                    break;
                case (UserActionType.RotateLeft):
                    WebApiApplication.DestroyerGame.RotateLeft(player);
                    break;
                case (UserActionType.RotateRight):
                    WebApiApplication.DestroyerGame.RotateRight(player);
                    break;
                case (UserActionType.Thrust):
                    WebApiApplication.DestroyerGame.Thrust(player);
                    break;
                default:
                    break;
            }
        }
    }

    public class DestroyrPlayer
    {
        public int PlayerId { get; set; }
        public string ConnectionId { get; set; }
        public string Name { get; set; }
    }
}