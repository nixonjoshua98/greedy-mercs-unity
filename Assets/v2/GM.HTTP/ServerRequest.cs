namespace GM.HTTP
{
    public interface IServerRequest
    {

    }

    public interface IAuthorisedServerRequest : IServerRequest
    {
        string DeviceId { get; set; }
    }

    public class AuthorisedServerRequest : IAuthorisedServerRequest
    {
        public string DeviceId { get; set; }
    }
}