using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Destroyer.Actions;
using Destroyer.Game;
using FFCG.Brun7;
using Microsoft.AspNet.SignalR;

namespace FFCG.Gamr.Destroyr
{
    public class DestroyrRunner
    {
        public Destroyer.Game.GameEngine Game;
        public readonly object UpdateLock = new object();
        private bool _hasCompletedUpdate = true;
        public List<DestroyrPlayer> Players;
        public List<DestroyrAction> Actions;
        private System.Threading.Timer _timer;

        public event EventHandler<object> Updated;

        private int _updateTicks = 0;

        public DestroyrRunner()
        {
            Game = GameBuilder.NewGame();
            Players = new List<DestroyrPlayer>();
            Actions = new List<DestroyrAction>();
            Game.StartLevel(1);

            _timer = new System.Threading.Timer(UpdateGameState, null, 0, 1000/60);
        }

        private void UpdateGameState(dynamic state)
        {
            lock (UpdateLock)
            {
                if( !_hasCompletedUpdate ) return;
                _hasCompletedUpdate = false;
            }

            //Debug.WriteLine("Updating world {0}", Game.Timer.Elapsed());

            DestroyrAction[] actions;
            lock (UpdateLock)
            {
                actions = Actions.ToArray();
                Actions = new List<DestroyrAction>();
            }

            foreach (var action in actions)
            {
                var player = Game.Players.FirstOrDefault(p => p.Id == action.PlayerId);
                switch (action.Action)
                {
                    case (UserActionType.Fire):
                        Game.ShootProjectile(player);
                        break;
                    case (UserActionType.RotateLeft):
                        Game.RotateLeft(player);
                        break;
                    case (UserActionType.RotateRight):
                        Game.RotateRight(player);
                        break;
                    case (UserActionType.Thrust):
                        Game.Thrust(player);
                        break;
                    default:
                        break;
                }
            }
            Game.RunOne();

            lock (UpdateLock)
            {
                _hasCompletedUpdate = true;
            }

            if (Updated != null)
            {
                //Debug.Write(".");
                Updated.Invoke(this, CreateGameState(this.Game));
            }
            else
            {
                //Debug.Write("-");
            }
        }

        public void Join(DestroyrPlayer destroyrPlayer)
        {
            lock (UpdateLock)
            {
                var player = Game.AddPlayer(destroyrPlayer.Name);
                destroyrPlayer.PlayerId = player.Id;

                Players.Add(destroyrPlayer);
            }
        }

        public object CreateGameState(GameEngine game)
        {
            return new
            {
                World = new
                {
                    Width = game.Board.Size.Width,
                    Height = game.Board.Size.Height,
                    GameObjects = game.Board.AllItems.Select(i => new
                    {
                        Name = i is Player ? ((Player)i).Name : i is Projectile ? "" : i.GetType().Name,
                        Position = new { X = i.Center.X, Y = i.Center.Y },
                        Shape = i.Geometry.Select(g => new { X = g.X, Y = g.Y }).ToArray(),
                        Rotation = i.Rotation,
                        Velocity = new { X = i.Velocity.X, Y = i.Velocity.Y }
                    }).ToArray()
                }
            };
        }

        public void UserAction(UserActionType action, DestroyrPlayer player)
        {
            lock (UpdateLock)
            {
                if(!Actions.Any(a => a.PlayerId == player.PlayerId && action == a.Action))
                    Actions.Add(new DestroyrAction() { Action = action, PlayerId = player.PlayerId });
            }
        }
    }


    public class DestroyrHub : Hub
    {
        public readonly object Lock = new object();

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            lock (Lock)
            {
                if (WebApiApplication.DestroyerGame != null)
                {
                    //WebApiApplication.DestroyerGame.Updated -= DestroyerGameOnUpdated;
                }
            }

            //Debug.WriteLine(String.Format("Disposed {0}", Context.ConnectionId));
        }

        private void DestroyerGameOnUpdated(object sender, object o)
        {
            Clients.All.updateState((dynamic)o);
        }

        public void CreateGame()
        {
            lock (Lock)
            {
                if (WebApiApplication.DestroyerGame == null)
                {
                    WebApiApplication.DestroyerGame = new DestroyrRunner();
                }
                //Debug.WriteLine(String.Format("Created {0}", Context.ConnectionId));
            }
        }

        public void View()
        {
            lock (Lock)
            {
                var game = WebApiApplication.DestroyerGame;
                if (game == null) return;

                game.Updated += DestroyerGameOnUpdated;
                //Debug.WriteLine(String.Format("Viewer {0}", Context.ConnectionId));
            }
        }

        public void Join(string playerName)
        {
            lock (Lock)
            {
                var game = WebApiApplication.DestroyerGame;
                if (game == null) return;

                game.Updated += DestroyerGameOnUpdated;

                var destroyrPlayer = new DestroyrPlayer() { ConnectionId = this.Context.ConnectionId, Name = playerName };
                WebApiApplication.DestroyerGame.Join(destroyrPlayer);
                //Debug.WriteLine(String.Format("Joined {0}", Context.ConnectionId));
            }
        }

        public void UserAction(UserActionType action)
        {
            lock (Lock)
            {
                var game = WebApiApplication.DestroyerGame;
                if (game == null) return;

                var destroyrPlayer = game.Players.FirstOrDefault(p => p.ConnectionId == this.Context.ConnectionId);

                //Debug.WriteLine(String.Format("User action {0} {1}", action, Context.ConnectionId));
                game.UserAction(action, destroyrPlayer);
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