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
    }
}