using System;
using Destroyer.TwoD;

namespace Destroyer.Game
{
    public class Item
    {
        public int Id { get; set; }
        public ItemStatus Status;
        public Point Center;
        public Point[] Geometry;
        public Rect BoundingBox;

        public virtual void UpdateBoundingBox()
        {
            var minX = float.MaxValue;
            var maxX = float.MinValue;
            var minY = float.MaxValue;
            var maxY = float.MinValue;

            foreach (var point in this.Geometry)
            {
                var x = this.Center.X + point.X;
                var y = this.Center.Y + point.Y;
                minX = Math.Min(x, minX);
                minY = Math.Min(x, minY);
                maxX = Math.Max(x, maxX);
                maxY = Math.Max(x, maxY);
            }

            BoundingBox = new Rect()
            {
                TopLeft = new Point() { X = minX, Y = minY },
                BottomRight = new Point() { X = maxX, Y = maxY }
            };
        }

        public virtual void UpdatePhysics(float elapsed, Board board)
        {

        }
    }
}