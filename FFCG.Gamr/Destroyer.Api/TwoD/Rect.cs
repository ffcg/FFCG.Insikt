namespace Destroyer.TwoD
{
    public struct Rect
    {
        public Rect(float x1, float y1, float x2, float y2)
        {
            this.TopLeft = new Point() { X = x1, Y = y1 };
            this.BottomRight = new Point() { X = x2, Y = y2 };
        }

        public Point TopLeft;
        public Point BottomRight;

        public float Width
        {
            get { return BottomRight.X - TopLeft.X; }
        }
        public float Height
        {
            get { return BottomRight.Y - TopLeft.Y; }
        }
    }
}