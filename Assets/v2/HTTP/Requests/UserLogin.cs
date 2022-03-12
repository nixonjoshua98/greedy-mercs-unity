using GM.Common.Models;
using Newtonsoft.Json;

namespace GM.HTTP.Requests
{
    public class LoginRequest : IServerRequest
    {
        public string DeviceId;

        public LoginRequest(string device)
        {
            DeviceId = device;
        }
    }


    public class LoginResponse : ServerResponse
    {
        [JsonProperty(Required = Required.Always)]
        public string Token { get; set; }

        public ServerUserDataModel UserData { get; set; }
    }
}
