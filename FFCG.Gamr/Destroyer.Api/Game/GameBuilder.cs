using System;
using Destroyer.TwoD;

namespace Destroyer.Game
{
    public static class GameBuilder
    {
        private readonly static Random Random;

        static GameBuilder()
        {
            Random = new Random(Environment.TickCount);
        }

        public static GameEngine NewGame()
        {
            var timer = new Timer();
            var game = new GameEngine(timer);
            return game;
        }

        public static Player AddPlayer(this GameEngine game, string name)
        {
            var player = new Player();
            player.Id = game.NextId++;
            player.Name = name;
            game.Players.Add(player);
            game.Board.AllItems.AddRange(game.Players);

            player.Geometry = new Point[]
            {
                new Point(){X = -5.0f, Y = -5.0f}, 
                new Point(){X = 5.0f, Y = 0.0f}, 
                new Point(){X = -5.0f, Y = 5.0f}, 
            };

            return player;
        }


        public static Board StartLevel(this GameEngine game, int level)
        {
            game.Board = new Board();
            game.Board.AllItems.AddRange(game.Players);
            game.Board.Level = level;
            game.Board.Size = new Rect(0, 0, 100, 100);

            // TODO: Create obstacles per level

            // TODO: Init players

            foreach (var player in game.Players)
            {
                player.Center = Random.NewPoint(game.Board.Size);
                player.Velocity = Random.NewVector(game.Board.Size.Width, game.Board.Size.Width);
            }


            return game.Board;
        }


    }
}