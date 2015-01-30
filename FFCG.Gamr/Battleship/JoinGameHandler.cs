using Microsoft.AspNet.SignalR;

namespace Battleship
{
    public class JoinGame : IRequest
    {
        public string Name { get; set; }
    }

    public class JoinGameHandler: IHandleRequests<JoinGame>
    {
        public void Handle(JoinGame request)
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
        }
    }
}
