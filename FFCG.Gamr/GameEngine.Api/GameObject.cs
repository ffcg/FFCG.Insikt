using System.Collections.Generic;

namespace GameEngine.Messages
{
    public class GameObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Point Position { get; set; }
        public List<Point> Shape { get; set; }
        public float Rotation { get; set; }
        public Vector Velocity { get; set; }
    }
 
    public class Point
    {
        public float X { get; set; }
        public float Y { get; set; }
    }

    public class Vector
    {
        public float X { get; set; }
        public float Y { get; set; }
    }

}