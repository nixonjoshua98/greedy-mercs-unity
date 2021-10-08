namespace GM.HTTP
{
    public interface IServerRequest
    {

    }


    public interface IAuthorisedRequest : IServerRequest
    {
        string DeviceId { get; set; }
        string UserId { get; set; }
    }


    public class AuthorisedRequest : IAuthorisedRequest
    {
        public string UserId { get; set; }
        public string DeviceId { get; set; }
    }
}