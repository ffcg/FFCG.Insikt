using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using System.Web;
using System.Web.Hosting;
using FFCG.Brun7;
using Microsoft.AspNet.SignalR;

namespace FFCG.Gamr.Brun7
{
    public class Brun7Hub : Hub
    {
        public void Hello()
        {
            Clients.All.listGames(WebApiApplication.BingoGames);
        }

        public void CreateGame(string playerName)
        {
            var roomId = Guid.NewGuid().ToString();

            Groups.Add(Context.ConnectionId, roomId);

            var bingoGame = new BingoGame(roomId);
            var bingoPlayer = new BingoPlayer(Context.ConnectionId, playerName);
            bingoGame.AddPlayer(bingoPlayer);
            WebApiApplication.BingoGames.Add(bingoGame);

            Clients.Group(roomId).roomCreated(roomId, bingoGame.Players);
            Clients.AllExcept(Context.ConnectionId).listGames(WebApiApplication.BingoGames);
        }

        public void StartGame(string gameId)
        {

            var game = WebApiApplication.BingoGames.FirstOrDefault(x => x.RoomId == gameId.ToString());
            game.StartGame(Clients);

        }

        
    }
}