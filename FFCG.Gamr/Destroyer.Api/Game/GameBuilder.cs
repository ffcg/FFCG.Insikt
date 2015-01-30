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

            player.Geometry = new Point[]
            {
                new Point(){X = -5.0f, Y = -5.0f}, 
                new Point(){X = 5.0f, Y = 0.0f}, 
                new Point(){X = -5.0f, Y = 5.0f}, 
            };

            return player;
        }

        public static Projectile ShootProjectile(this GameEngine game, Motile origin)
        {
            var projectile = new Projectile();
            projectile.Id = game.NextId++;
            projectile.Center = origin.Center;
            projectile.Rotation = origin.Rotation;

            projectile.Geometry = new Point[]
            {
                new Point(){X = -0.5f, Y = -0.5f}, 
                new Point(){X = 0.5f, Y = 0.0f}, 
                new Point(){X = -0.5f, Y = 0.5f}, 
            };
            var speed = 50.0f;
            projectile.Velocity = new Vector()
            {
                X = 10.0f * (float)Math.Cos(projectile.Rotation) * speed,
                Y = 10.0f * (float)Math.Cos(projectile.Rotation) * speed
            };

            game.Board.AllItems.Add(projectile);

            return projectile;
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