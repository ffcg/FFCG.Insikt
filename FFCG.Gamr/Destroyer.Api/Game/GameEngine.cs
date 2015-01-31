using System;
using System.Collections.Generic;
using System.Diagnostics;
using Destroyer.TwoD;

namespace Destroyer.Game
{

    public static class GameConsoleRenderer
    {
        public static void Render(this GameEngine game, ITimer timer)
        {
            
            //Console.Clear();
            /*
            Debug.WriteLine(@"                                                         _      _                                     ");
            Debug.WriteLine(@" _______   _______     _______.___________..______       U______U  ____    ____  _______ .______      ");
            Debug.WriteLine(@"|       \ |   ____|   /       |           ||   _  \      /  __  \  \   \  /   / |   ____||   _  \     ");
            Debug.WriteLine(@"|  .--.  ||  |__     |   (----`---|  |----`|  |_)  |    |  |  |  |  \   \/   /  |  |__   |  |_)  |    ");
            Debug.WriteLine(@"|  |  |  ||   __|     \   \       |  |     |      /     |  |  |  |   \_    _/   |   __|  |      /     ");
            Debug.WriteLine(@"|  '--'  ||  |____.----)   |      |  |     |  |\  \----.|  `--'  |     |  |     |  |____ |  |\  \----.");
            Debug.WriteLine(@"|_______/ |_______|_______/       |__|     | _| `._____| \______/      |__|     |_______|| _| `._____|");
            */
            Debug.WriteLine("");
            Debug.WriteLine("Level: {0}, Run {1}", game.Board.Level, timer.Ticks());
            Debug.WriteLine("======================================");

            foreach (var item in game.Board.AllItems)
            {
                item.Render();
            }

            //game.RenderScene();

            game.RenderCollisions();
        }

        public static void Render(this Item item)
        {
            Debug.WriteLine("Item: {0} [{1}]", item.Id, item.GetType().Name);
            Debug.WriteLine("Pos: {0:N0} {1:N0} {2:N0} {3:N0} {4:N0} ", item.Center.X, item.Center.Y, item.Rotation * 180 / Math.PI, item.Velocity.X, item.Velocity.Y);
            Debug.WriteLine("BB:  {0:N0} {1:N0} - {2:N0} {3:N0} ", item.BoundingBox.TopLeft.X, item.BoundingBox.TopLeft.Y, item.BoundingBox.BottomRight.X, item.BoundingBox.BottomRight.Y);
            Debug.WriteLine("------------------------------------");
        }

        public static void RenderScene(this GameEngine game)
        {
            var scene = new char[(int)game.Board.Size.Width, (int)game.Board.Size.Height];

            foreach (var item in game.Board.AllItems)
            {
                for (var i = item.BoundingBox.TopLeft.X; i < item.BoundingBox.BottomRight.X; i++)
                {
                    for (var j = item.BoundingBox.TopLeft.Y; j < item.BoundingBox.BottomRight.Y; j++)
                    {
                        if (((int)i > 0) && ((int)i < game.Board.Size.Width) && ((int)j > 0) && ((int)j < game.Board.Size.Height))
                        {
                            scene[(int)i, (int)j] = item.Id.ToString()[0];
                        }
                    }
                }
            }

            Console.WriteLine("________________________________________________________________________");
            for (var i = game.Board.Size.TopLeft.X; i < game.Board.Size.BottomRight.X; i++)
            {
                for (var j = game.Board.Size.TopLeft.Y; j < game.Board.Size.BottomRight.Y; j++)
                {
                    if (((int)i > 0) && ((int)i < game.Board.Size.Width) && ((int)j > 0) && ((int)j < game.Board.Size.Height))
                    {
                        Debug.Write(String.Format("{0}", scene[(int) i, (int) j]));
                    }
                }
                Debug.WriteLine("");
            }
            Debug.WriteLine("________________________________________________________________________");
        }

        public static void RenderCollisions(this GameEngine game)
        {
            foreach (var collision in game.Collisions)
            {
                Debug.WriteLine("Collision {0} {1}", collision.A.Id, collision.B.Id);
            }
        }
    }
    public class GameEngine
    {
        private readonly ITimer _timer;

        private const float Rad360 = 2.0f * (float)Math.PI;

        public ITimer Timer
        {
            get { return _timer; }
        }

        public GameEngine(ITimer timer)
        {
            _timer = timer;
            Players = new List<Player>();
            Board = new Board();

            NextId = 0;
        }

        public Board Board;
        public List<Player> Players;

        public List<Collision> Collisions;

        public int NextId;

        public void RunOne()
        {
            _timer.Update();
            foreach (var item in Board.AllItems)
            {
                item.UpdateBoundingBox();
            }

            foreach (var item in Board.AllItems)
            {
                item.UpdatePhysics(_timer.Elapsed(), Board);
            }

            Collisions = new List<Collision>(UpdateCollisions());

            foreach (var item in Board.AllItems.ToArray())
            {
                if (item.Status == ItemStatus.Dead)
                {
                    Board.AllItems.Remove(item);
                }
            }

            //this.Render(_timer);
        }

        public IEnumerable<Collision> UpdateCollisions()
        {
            foreach (var item in Board.AllItems)
            {
                if (item is Motile)
                {
                    foreach (var checkItem in Board.AllItems)
                    {
                        if (item != checkItem && IsOverlapping(item.BoundingBox, checkItem.BoundingBox))
                        {
                            yield return new Collision() { A = item, B = checkItem };
                        }
                    }
                }
            }
        }

        private bool IsOverlapping(Rect rect1, Rect rect2)
        {
            return !(rect2.TopLeft.X > rect1.BottomRight.X
                     || rect2.BottomRight.X < rect1.TopLeft.X
                     || rect2.TopLeft.Y > rect1.BottomRight.Y
                     || rect2.BottomRight.Y < rect1.TopLeft.Y);
        }

        public Projectile ShootProjectile(Player player)
        {
            var projectile = new Projectile();
            projectile.Id = this.NextId++;
            projectile.Center = player.Center;
            projectile.Rotation = player.Rotation;

            projectile.Geometry = new Point[]
            {
                new Point(){X = -0.5f, Y = -0.5f}, 
                new Point(){X = 0.5f, Y = 0.0f}, 
                new Point(){X = -0.5f, Y = 0.5f}, 
            };
            var speed = 5.0f;
            projectile.Velocity = new Vector()
            {
                X = 10.0f * (float)Math.Cos(projectile.Rotation) * speed,
                Y = 10.0f * (float)Math.Sin(projectile.Rotation) * speed
            };

            this.Board.AllItems.Add(projectile);

            return projectile;
        }

        public void RotateLeft(Player player)
        {
            player.Rotation -= 5.0f * (float)Math.PI / 180.0f;
            if (player.Rotation < 0)
            {
                player.Rotation += Rad360;
            }
        }

        public void RotateRight(Player player)
        {
            player.Rotation += 5.0f * (float)Math.PI / 180.0f;
            if (player.Rotation > Rad360)
            {
                player.Rotation -= Rad360;
            }
        }

        public void Thrust(Player player)
        {
            var newVelocity = new Vector()
            {
                X = player.Velocity.X + .2f*(float) Math.Cos(player.Rotation),
                Y = player.Velocity.Y + .2f * (float)Math.Sin(player.Rotation)
            };
            player.Velocity = newVelocity;
        }
    }
}