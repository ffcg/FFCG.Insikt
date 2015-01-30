using System.Collections.Generic;

namespace GameEngine.Api
{
    public class GameObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Point Position { get; set; }
        public List<Point> Body { get; set; }
        public float Rotation { get; set; }
    }
 
    public class Point
    {
        public float X { get; set; }
        public float Y { get; set; }
    }

}