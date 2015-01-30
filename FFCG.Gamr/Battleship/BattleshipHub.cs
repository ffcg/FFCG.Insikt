using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Battleship
{
    public class BattleshipHub : Hub
    {
        public void Handshake()
        {
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        //public void NotifyThatGameIsWaitingForSecondPlayer(Guid gameId)
        //{
        //    Clients.All.gameIsWaitingForSecondPlayer(gameId);
        //}

        //public void NotifyThatGameIsReadyToStart(Guid gameId)
        //{
        //    Clients.All.gameIsReadyToStart(gameId);
        //}
    }
}