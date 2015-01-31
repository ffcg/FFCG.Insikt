using System;
using Microsoft.AspNet.SignalR.Client;

namespace FFCG.Brun7.Retro
{
    class Program
    {
        static void Main(string[] args)
        {
            var brun7Hub = new HubConnection("http://insiktweb.azurewebsites.net");
            var proxy = brun7Hub.CreateHubProxy("brun7Hub");

            brun7Hub.Start().Wait();

            Console.WriteLine("Enter your name:");
            var playerName = Console.ReadLine();
            proxy.Invoke("JoinGame", new object[] { playerName });

            proxy.On<object, object>("refreshCurrentGameState", (currentNumber, game) =>
            {
                Console.Write(currentNumber + " ");

            });

            proxy.On<string>("announceBingoWinner", (bingoWinner) =>
            {
                Console.Write("\n\nBingo winner: " + bingoWinner);

            });

            while (true)
            {
                
            }
        }
    }
    
}
