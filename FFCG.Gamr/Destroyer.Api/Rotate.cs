using GameEngine.Messages;

namespace Destroyer.Messages
{
    public class Rotate : UserAction
    {
        public float RotationDelta { get; set; }
    }
}
