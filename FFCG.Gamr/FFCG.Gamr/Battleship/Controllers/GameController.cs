using System.Web.Http;
using Battleship;

namespace FFCG.Gamr.Battleship.Controllers
{
    public class GameController : ApiController
    {
        public void Join(string value)
        {
            var joinGameHandler = new JoinGameHandler();
            joinGameHandler.Handle(new JoinGame() { Name = value });
        }
    }
}
