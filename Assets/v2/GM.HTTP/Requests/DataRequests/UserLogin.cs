using Newtonsoft.Json;

namespace GM.HTTP.Requests
{
    public class UserLoginRequest : IServerRequest
    {
        public string DeviceId;

        public UserLoginRequest(string device)
        {
            DeviceId = device;
        }
    }


    public class UserLoginReponse : ServerResponse, IServerAuthentication
    {
        [JsonRequired]
        [JsonProperty(PropertyName = "sessionId")]
        public string Session { get; set; }
    }
}
