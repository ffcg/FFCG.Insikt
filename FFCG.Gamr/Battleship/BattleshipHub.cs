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
    }
}