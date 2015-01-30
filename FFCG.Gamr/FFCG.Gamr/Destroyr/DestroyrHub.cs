using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace FFCG.Gamr.Destroyr
{
    public class DestroyrHub : Hub
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