﻿using System;
using System.Web.Http;
using Battleship;

namespace FFCG.Gamr.Battleship.Controllers
{
    public class GameController : ApiController
    {
        public void Join(string value)
        {
            var joinGameHandler = new JoinGameHandler();
            joinGameHandler.Handle(new JoinGame { Name = value });
        }

        public void PlaceShip(PlaceShipViewModel value)
        {
            var handler = new PlaceShipHandler();
            handler.Handle(new PlaceShip
            {
                PlayerId = value.PlayerId,
                X = value.X,
                Y = value.Y,
            });
        }
    }

    public class PlaceShipViewModel
    {
        public Guid PlayerId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
