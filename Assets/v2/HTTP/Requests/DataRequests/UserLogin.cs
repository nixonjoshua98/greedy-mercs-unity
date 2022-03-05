using GM.Common.Models;
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
        [JsonProperty(PropertyName = "sessionId", Required = Required.Always)]
        public string Session { get; set; }

        public ServerUserDataModel UserData { get; set; }
    }
}
