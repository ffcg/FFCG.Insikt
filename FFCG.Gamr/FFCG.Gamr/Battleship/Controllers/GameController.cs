using System.Collections.Generic;
using System.Web.Http;
using Battleship;

namespace FFCG.Gamr.Battleship.Controllers
{
    public class GameController : ApiController
    {
        public void Post(string playerAlias)
        {
            var joinGameHandler = new JoinGameHandler();
            var player = joinGameHandler.Handle(new JoinGame() { Name = playerAlias });
        }
    }
}
