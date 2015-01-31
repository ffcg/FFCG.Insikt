using System;
using System.Web.Http;
using Battleship;

namespace FFCG.Gamr.Battleship.Controllers
{
    public class GameController : ApiController
    {
        public Guid Join(string value)
        {
            var joinGameHandler = new JoinGameHandler();
            var playerViewModel = joinGameHandler.Handle(new JoinGame { Name = value });
            return playerViewModel.PlayerId;
        }

        public void PlaceShip([FromBody]PlaceShipInputModel value)
        {
            var handler = new PlaceShipHandler();
            handler.Handle(new PlaceShip
            {
                PlayerId = value.PlayerId,
                X = value.X,
                Y = value.Y,
            });
        }

        public void Fire(PlaceShipInputModel value)
        {
            var handler = new FireHandler();
            handler.Handle(new Fire
            {
                PlayerId = value.PlayerId,
                X = value.X,
                Y = value.Y,
            });
        }
    }

    public class PlaceShipInputModel
    {
        public Guid PlayerId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
