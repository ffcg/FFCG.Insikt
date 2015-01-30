using System;

namespace Battleship
{
    public class ShootCellResponse
    {
        bool Success { get; set; }
    }

    public class ShootCell : IRequest<ShootCellResponse>
    {

    }

    public class ShootCellHandler : IHandleRequests<ShootCell, ShootCellResponse>
    {
        public ShootCellResponse Request(ShootCell request)
        {
            throw new NotImplementedException();
        }
    }
}