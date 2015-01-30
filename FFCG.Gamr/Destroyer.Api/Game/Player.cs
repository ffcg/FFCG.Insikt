using Destroyer.TwoD;

namespace Destroyer.Game
{
    public class Player : Motile
    {
        public Player()
        {
            Status = ItemStatus.Alive;
        }

        public string Name;

        public override void UpdatePhysics(float elapsed, Board board)
        {
            var x = this.Center.X + this.Velocity.X * elapsed;
            var y = this.Center.Y + this.Velocity.Y * elapsed;

            if (x > board.Size.Width) x = (x - board.Size.Width);
            if (y > board.Size.Height) y = (y - board.Size.Height);
            if (x < 0) x = (board.Size.Width - x);
            if (y < 0) x = (board.Size.Width - y);

            this.Center = new Point() { X = x, Y = y };
        }
    }
}