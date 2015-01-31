using System;
using Battleship.Domain;
using Microsoft.AspNet.SignalR;

namespace Battleship
{
    public class Fire : IRequest<object>
    {
        public Guid PlayerId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class FireHandler : IHandleRequests<Fire, object>
    {
        public object Handle(Fire request)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<BattleshipHub>();

            var game = GameController.Get().GetCurrentGame();
            
            var player = game.GetPlayer(request.PlayerId);
            var enemy = game.GetEnemyOf(request.PlayerId);
            var success = game.Fire(player.Id,enemy.Id,new Cell(request.X, request.Y));

            if (success)
            {
                if (enemy.AllShipsAreSunk)
                {
                    var winner = player.Id;
                    hubContext.Clients.All.gameOver(winner);
                    return null;
                }

                hubContext.Clients.All.playerSunkShip(player.Id, enemy.Id, request.X, request.Y);
            }
            else
            {
                hubContext.Clients.All.playerMissed(player.Id, enemy.Id, request.X, request.Y);
            }

            var activePlayer = game.GetActivePlayer();

            if (activePlayer.Id != player.Id)
            {
                hubContext.Clients.All.newActivePlayer(activePlayer.Id);
            }

            return null;
        }
    }
}