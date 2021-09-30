using Newtonsoft.Json;

namespace GM.HTTP.Models
{
    public interface IServerResponse
    {
        long StatusCode { get; set; }
        string ErrorMessage { get; set; }
    }


    public class ServerResponse : IServerResponse
    {
        [JsonIgnore]
        public long StatusCode { get; set; }

        [JsonProperty(PropertyName = "error")]
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
