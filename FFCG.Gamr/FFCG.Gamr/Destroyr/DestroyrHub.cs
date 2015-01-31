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
        public readonly object Lock = new object();
        public static List<DestroyrPlayer> Players;
        public static List<DestroyrAction> Actions;
        private System.Threading.Timer _timer;

        public void CreateGame()
        {
            lock (Lock)
            {
                if (WebApiApplication.DestroyerGame == null)
                {
                    WebApiApplication.DestroyerGame = GameBuilder.NewGame();
                    Players = new List<DestroyrPlayer>();
                    Actions = new List<DestroyrAction>();
                    WebApiApplication.DestroyerGame.StartLevel(1);

                    _timer = new System.Threading.Timer(UpdateGameState, null, 0, 25);
                }
            }
        }

        private void UpdateGameState(dynamic state)
        {
            DestroyrAction[] actions;
            lock (Lock)
            {
                actions = Actions.ToArray();
                Actions = new List<DestroyrAction>();
            }

            var game = WebApiApplication.DestroyerGame;

            foreach (var action in actions)
            {
                var player = game.Players.FirstOrDefault(p => p.Id == action.PlayerId);
                switch (action.Action)
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

            Clients.All.updateState(CreateGameState(game));
        }

        private dynamic CreateGameState(GameEngine game)
        {
            return new
            {
                World = new
                {
                    Width = game.Board.Size.Width,
                    Height = game.Board.Size.Height,
                    GameObjects = game.Board.AllItems.Select(i => new
                    {
                        Name = i.GetType().Name,
                        Position = new {X = i.Center.X, Y = i.Center.Y},
                        Shape = i.Geometry.Select(g => new {X = g.X, Y = g.Y}).ToArray(),
                        Rotation = i.Rotation,
                        Velocity = new {X = i.Velocity.X, Y = i.Velocity.Y}
                    }).ToArray()
                }
            };
        }

        public void Join(string playerName)
        {
            lock (Lock)
            {
                var destroyrPlayer = new DestroyrPlayer() { ConnectionId = this.Context.ConnectionId, Name = playerName };
                var player = WebApiApplication.DestroyerGame.AddPlayer(playerName);
                destroyrPlayer.PlayerId = player.Id;

                Players.Add(destroyrPlayer);
            }
        }

        public void UserAction(UserActionType action)
        {
            var game = WebApiApplication.DestroyerGame;
            var destroyrPlayer = Players.FirstOrDefault(p => p.ConnectionId == this.Context.ConnectionId);

            lock (Lock)
            {
                Actions.Add(new DestroyrAction() {Action = action, PlayerId = destroyrPlayer.PlayerId});
            }
        }
    }

    public class DestroyrAction
    {
        public int PlayerId { get; set; }
        public UserActionType Action { get; set; }
    }

    public class DestroyrPlayer
    {
        public int PlayerId { get; set; }
        public string ConnectionId { get; set; }
        public string Name { get; set; }
    }
}