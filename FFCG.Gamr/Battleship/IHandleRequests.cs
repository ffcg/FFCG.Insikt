namespace Battleship
{
    public interface IHandleRequests<in TRequest, out TResponse>
    {
        TResponse Handle(TRequest request);
    }
}