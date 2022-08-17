using Newtonsoft.Json;
using SRC.Models;

namespace SRC.HTTP.Requests
{
    public class LoginResponse : ServerResponse
    {
        [JsonProperty(Required = Required.Always)]
        public string Token { get; set; }

        public ServerUserDataModel UserData { get; set; }
    }
}
