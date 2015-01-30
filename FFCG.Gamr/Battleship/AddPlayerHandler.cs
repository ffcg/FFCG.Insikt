using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class AddedPlayerViewModel
    {
        public Guid PlayerId { get; set; }
        public string Name { get; set; }
    }

    public class AddPlayer : IRequest<AddedPlayerViewModel>
    {
        public string Name { get; set; }
    }

    public class AddPlayerHandler: IHandleRequests<AddPlayer, AddedPlayerViewModel>
    {
        public AddedPlayerViewModel Request(AddPlayer request)
        {
            //add player to game
            //return added player info
            //if other player exists, use signal r to notify both that game is ready to start
            //if no other player exists, notify player that he needs to wait
            return null;
        }
    }
}
