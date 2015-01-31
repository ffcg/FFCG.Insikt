using System;
using Microsoft.AspNet.SignalR;

namespace Battleship
{
    public class PlaceShip : IRequest<object>
    {
        public Guid PlayerId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class PlaceShipHandler : IHandleRequests<PlaceShip, object>
    {
        public object Handle(PlaceShip request)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<BattleshipHub>();

            var game = GameController.Get().GetCurrentGame();

            var player = game.GetPlayer(request.PlayerId);
            player.PlaceShip(request.X, request.Y);

            hubContext.Clients.All.shipHasBeenPlaced(game.Id, player.Id, request.X, request.Y);

            if (game.AllShipsArePlaced)
            {
                var activePlayer = game.GetActivePlayer();
                hubContext.Clients.All.allShipsHaveBeenPlaced(game.Id, activePlayer.Id);
            }

            return null;
        }
    }
}
