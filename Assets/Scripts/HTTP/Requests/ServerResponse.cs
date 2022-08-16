namespace SRC.HTTP.Requests
{
    public interface IServerResponse
    {
        long StatusCode { get; set; }
        string Message { get; set; }
    }


    public class ServerResponse : IServerResponse
    {
        public long StatusCode { get; set; } = 0;
        public string Message { get; set; } = string.Empty;

        public bool IsSuccess => StatusCode == HTTPCodes.Success;
    };
}
