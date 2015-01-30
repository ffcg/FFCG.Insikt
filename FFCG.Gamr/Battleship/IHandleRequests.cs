namespace Battleship
{
    public interface IHandleRequests<in TRequest, out TResponse>
    {
        TResponse Request(TRequest request);
    }
}