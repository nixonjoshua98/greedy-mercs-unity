namespace GM.HTTP
{
    public interface IServerRequest
    {

    }

    public sealed class ServerRequest : IServerRequest
    {
        public static ServerRequest Empty = new();
    }
}