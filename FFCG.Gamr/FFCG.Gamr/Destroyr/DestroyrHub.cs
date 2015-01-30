using System;
using System.Linq;
using System.Threading.Tasks;
using Destroyer.Actions;
using Destroyer.Game;
using FFCG.Brun7;
using Microsoft.AspNet.SignalR;

namespace FFCG.Gamr.Destroyr
{
    public class DestroyrHub : Hub
    {

        public void CreateGame()
        {
            if (WebApiApplication.DestroyerGame == null)
            {
                WebApiApplication.DestroyerGame = GameBuilder.NewGame();
            }
        }

        public void StartGame()
        {
            WebApiApplication.DestroyerGame.StartLevel(1);
        }

        public void UserAction(UserActionType action)
        {
            switch (action)
            {
                case (UserActionType.Fire):
                    break;
                case (UserActionType.RotateLeft):
                    break;
                case (UserActionType.RotateRight):
                    break;
                case (UserActionType.Thrust):
                    break;
                default:
                    break;
            }
        }

    }
}