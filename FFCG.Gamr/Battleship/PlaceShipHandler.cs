using System;
using System.Linq;
using Microsoft.AspNet.SignalR;

namespace Battleship
{
    public class PlaceShip : IRequest
    {
        public Guid PlayerId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class PlaceShipHandler : IHandleRequests<PlaceShip>
    {
        public void Handle(PlaceShip request)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<BattleshipHub>();

            var game = GameController.Get().GetCurrentGame();
            
            var player = game.Players.First(x => x.Id == request.PlayerId);
            player.PlaceShip(request.X, request.Y);

            if (game.AllShipsArePlaced)
            {
                var activePlayer = game.GetActivePlayer();
                hubContext.Clients.All.allShipsHaveBeenPlaced(game.Id, activePlayer.Id);
            }
        }
    }
}
