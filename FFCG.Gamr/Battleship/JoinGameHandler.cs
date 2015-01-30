using System;
using Microsoft.AspNet.SignalR;

namespace Battleship
{
    public class AddedPlayerViewModel
    {
        public Guid GameId { get; set; }
        public Guid PlayerId { get; set; }
        public string Name { get; set; }
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
            
            var viewModel = new AddedPlayerViewModel
            {
                GameId = game.Id,
                PlayerId = player.Id,
                Name = player.Name,
            };

            if (game.IsWaitingForPlayers)
            {
                hubContext.Clients.All.gameIsWaitingForSecondPlayer(game.Id, player.Id);
            }
            else
            {
                hubContext.Clients.All.gameIsReadyToStart(game.Id, player.Id);
            }

            return viewModel;
        }
    }
}
