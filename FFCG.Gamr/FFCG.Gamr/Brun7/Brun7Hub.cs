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
            if(WebApiApplication.BingoGame != null)
                Clients.All.gameReady();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var connectionId = Context.ConnectionId;
            var bingoGame = WebApiApplication.BingoGame;
            bingoGame.RemovePlayer(connectionId);

            if (!bingoGame.Players.Any())
                WebApiApplication.BingoGame = null;
            else
            {
                Clients.Group(bingoGame.RoomId).playerJoined(bingoGame.RoomId, bingoGame.Players);    
            }
            
            return base.OnDisconnected(stopCalled);
        }

        public void CreateGame(string playerName)
        {
            var roomId = Guid.NewGuid().ToString();

            Groups.Add(Context.ConnectionId, roomId);

            var bingoGame = new BingoGame(roomId);
            var bingoPlayer = new BingoPlayer(Context.ConnectionId, playerName);
            bingoGame.AddPlayer(bingoPlayer);
            WebApiApplication.BingoGame = bingoGame;

            Clients.Group(roomId).roomCreated(roomId, bingoGame.Players);
            Clients.AllExcept(Context.ConnectionId).refreshGlobalStatus(WebApiApplication.BingoGame);
        }

        public void StartGame()
        {

            var game = WebApiApplication.BingoGame;
            game.StartGame(Clients);

        }

        public void ResetGame()
        {
            var game = WebApiApplication.BingoGame;
            game.Reset();
            game.StartGame(Clients);
            Clients.Group(game.RoomId).gameResetted();
        }

        public void JoinGame(string name)
        {
            var game = WebApiApplication.BingoGame;
            Groups.Add(Context.ConnectionId, game.RoomId);
            game.AddPlayer(new BingoPlayer(Context.ConnectionId, name));
            Clients.Group(game.RoomId).playerJoined(game.RoomId, game.Players);
        }

        
    }
}