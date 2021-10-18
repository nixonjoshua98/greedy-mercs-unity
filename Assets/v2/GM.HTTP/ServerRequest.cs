namespace GM.HTTP
{
    public interface IServerRequest
    {

    }


    public interface IAuthenticatedRequest : IServerRequest
    {
        string DeviceId { get; set; }
        string UserId { get; set; }
    }


    public class AuthenticatedRequest : IAuthenticatedRequest
    {
        public string UserId { get; set; }
        public string DeviceId { get; set; }
    }
}