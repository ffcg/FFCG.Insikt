using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFCG.Brun7.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var brun7Hub = new HubConnection("http://localhost:50843/");
            var proxy = brun7Hub.CreateHubProxy("brun7Hub");

            //proxy.Invoke("Hello").Wait();

            proxy.On<string>("JoinGame", (name) => System.Console.Write("Name: " + name));
            brun7Hub.Start().Wait();
        }
    }
    
}
