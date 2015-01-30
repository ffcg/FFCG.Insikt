using Destroyer.TwoD;

namespace Destroyer.Game
{
    public class Motile : Item
    {
        public Vector Velocity;
        public float Rotation;

        public override void UpdatePhysics(float elapsed, Board board)
        {
            var x = this.Center.X + this.Velocity.X * elapsed;
            var y = this.Center.Y + this.Velocity.Y * elapsed;
            this.Center = new Point() { X = x, Y = y };
        }
    }
}