using System;
using Destroyer.TwoD;

namespace Destroyer.Game
{
    public static class RandomHelper
    {
        public static Point NewPoint(this Random random, Rect bounds)
        {
            return new Point()
            {
                X = (float)random.NextDouble() * bounds.Width + bounds.TopLeft.X,
                Y = (float)random.NextDouble() * bounds.Height + bounds.TopLeft.Y
            };
        }

        public static Vector NewVector(this Random random, float maxX, float maxY)
        {
            return new Vector()
            {
                X = (float)random.NextDouble() * maxX,
                Y = (float)random.NextDouble() * maxY
            };
        }
    }
}