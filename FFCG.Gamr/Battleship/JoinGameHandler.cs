using System;
using Microsoft.AspNet.SignalR;

namespace Battleship
{
    public class AddedPlayerViewModel
    {
        public Guid PlayerId { get; set; }
    }

    public class JoinGame : IRequest<AddedPlayerViewModel>
    {
        public string Name { get; set; }
    }

    public class JoinGameHandler: IHandleRequests<JoinGame, AddedPlayerViewModel>
    {
        public AddedPlayerViewModel Handle(JoinGame request)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<BattleshipHub>();

            var controller = GameController.Get();
            var game = controller.GetCurrentGame();
            var player = game.AddPlayer(request.Name);
            
            if (game.IsWaitingForPlayers)
            {
                hubContext.Clients.All.gameIsWaitingForSecondPlayer(game.Id, player.Id);
            }
            else
            {
                game.Start();
                hubContext.Clients.All.gameHasBeenStarted(game.Id, player.Id);
            }

            return new AddedPlayerViewModel {PlayerId = player.Id};
        }
    }
}
