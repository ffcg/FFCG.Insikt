namespace Battleship
{
    public interface IHandleRequests<in TRequest> where TRequest : IRequest
    {
        void Handle(TRequest request);
    }
}