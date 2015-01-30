using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using FFCG.Brun7;
using Microsoft.AspNet.SignalR;

namespace FFCG.Gamr.Brun7
{
    public class Brun7Hub : Hub
    {
        private List<BingoGame> _games;

        public Brun7Hub()
        {
            _games = new List<BingoGame>();
        }

        public void Hello()
        {
            Clients.All.listGames(_games);
        }

        public void CreateGame(string playerName)
        {
            var roomId = Guid.NewGuid().ToString();

            Groups.Add(Context.ConnectionId, roomId);

            var bingoGame = new BingoGame(roomId);
            var bingoPlayer = new BingoPlayer(Context.ConnectionId, playerName);
            bingoGame.AddPlayer(bingoPlayer);
            _games.Add(bingoGame);

            Clients.Group(roomId).roomCreated(roomId, bingoGame.Players);
            Clients.AllExcept(Context.ConnectionId).listGames(_games);
        }
    }
}