using System;

namespace Battleship
{
    public class AddedPlayerViewModel
    {
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
                PlayerId = player.Id,
                Name = player.Name,
            };

            //if other player exists, use signal r to notify both that game is ready to start
            //if no other player exists, notify player that he needs to wait

            return viewModel;
        }
    }
}
