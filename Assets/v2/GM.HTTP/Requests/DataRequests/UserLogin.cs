using Newtonsoft.Json;

namespace GM.HTTP.Requests
{
    public interface ILoginRequest : IServerRequest
    {
        string DeviceId { get; set; }
    }

    public class UserLoginRequest : ILoginRequest
    {
        public string DeviceId { get; set; }
    }


    public class UserLoginReponse : ServerResponse, IServerAuthentication
    {
        [JsonRequired]
        public string UserId { get; set; }

        [JsonRequired]
        public string SessionId { get; set; }
    }
}
