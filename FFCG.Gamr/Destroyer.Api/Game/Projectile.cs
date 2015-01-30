using Destroyer.TwoD;

namespace Destroyer.Game
{
    public class Projectile : Motile
    {
        public Projectile()
        {
            Status = ItemStatus.Alive;
        }

        public override void UpdatePhysics(float elapsed, Board board)
        {
            var x = this.Center.X + this.Velocity.X * elapsed;
            var y = this.Center.Y + this.Velocity.Y * elapsed;

            if ((x > board.Size.Width) ||
                (y > board.Size.Height) ||
                (x < 0) ||
                (y < 0))
            {
                Status = ItemStatus.Dead;
            }

            this.Center = new Point() { X = x, Y = y };
        }
    }
}