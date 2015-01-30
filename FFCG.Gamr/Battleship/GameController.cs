using Battleship.Domain;

namespace Battleship
{
    public class GameController
    {
        private static readonly GameController Singleton = new GameController();
        private Game _game;

        private GameController()
        {}

        public static GameController Get()
        {
            return Singleton;
        }

        public Game GetCurrentGame()
        {
            return _game ?? (_game = new Game(5));
        }
    }
}