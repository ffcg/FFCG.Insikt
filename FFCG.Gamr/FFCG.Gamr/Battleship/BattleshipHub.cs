using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace FFCG.Gamr.Battleship
{
    public class BattleshipHub : Hub
    {
        public void Handshake()
        {
            
        }

        public override Task OnConnected()
        {
            var test = Clients.Caller;
            return base.OnConnected();
        }

    }
}