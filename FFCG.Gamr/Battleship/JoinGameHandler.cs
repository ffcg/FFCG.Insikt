using System;

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
            var game = GameController.Get().GetCurrentGame();
            var player = game.AddPlayer(request.Name);
            
            var viewModel = new AddedPlayerViewModel
            {
                GameId = game.Id,
                PlayerId = player.Id,
                Name = player.Name,
            };

            var hub = new BattleshipHub();

            if (game.IsWaitingForPlayers)
            {
                hub.NotifyThatGameIsWaitingForSecondPlayer(game.Id);
            }
            else
            {
                hub.NotifyThatGameIsReadyToStart(game.Id);
            }

            return viewModel;
        }
    }
}
