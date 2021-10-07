namespace GM.HTTP
{
    public interface IServerRequest
    {

    }

    public interface IPublicRequest : IServerRequest
    {

    }

    public interface IAuthorisedRequest : IServerRequest
    {
        string DeviceId { get; set; }
    }

    public class AuthorisedRequest : IAuthorisedRequest
    {
        public string DeviceId { get; set; }
    }
}