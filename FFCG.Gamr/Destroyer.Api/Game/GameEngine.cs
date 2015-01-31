using System;
using System.Collections.Generic;
using Destroyer.TwoD;

namespace Destroyer.Game
{
    public class GameEngine
    {
        private readonly ITimer _timer;

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
            foreach (var item in Board.AllItems)
            {
                item.UpdateBoundingBox();
            }

            foreach (var item in Board.AllItems)
            {
                item.UpdatePhysics(_timer.Elapsed(), Board);
            }

            Collisions = new List<Collision>(UpdateCollisions());

            foreach (var item in Board.AllItems)
            {
                if (item.Status == ItemStatus.Dead)
                {
                    Board.AllItems.Remove(item);
                }
            }
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

        public Projectile ShootProjectile(Motile origin)
        {
            var projectile = new Projectile();
            projectile.Id = this.NextId++;
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

            this.Board.AllItems.Add(projectile);

            return projectile;
        }

        public void RotateLeft(Player player)
        {
            
        }

        public void RotateRight(Player player)
        {
            
        }

        public void Thrust(Player player)
        {
            
        }
    }
}