using System;

namespace Battleship
{
    public class ShootCell : IRequest
    {

    }

    public class ShootCellHandler : IHandleRequests<ShootCell>
    {
        public void Handle(ShootCell request)
        {
            throw new NotImplementedException();
        }
    }
}