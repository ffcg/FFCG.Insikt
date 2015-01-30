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

            proxy.Invoke("StartGame", null).Wait();

            //proxy.On<string, string>("Hello",(name, message) => System.Console.Write("Message: " + name + " message: " + message));
            //brun7Hub.Start().Wait();
        }
    }
    
}
