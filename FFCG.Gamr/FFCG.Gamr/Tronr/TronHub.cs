using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace FFCG.Gamr.Tronr
{
    public class TronHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello("Hello from server");
        }

        public void Send()
        {
            Clients.All.hello("hello");
        }
    }
}