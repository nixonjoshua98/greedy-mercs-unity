using Newtonsoft.Json;

namespace GM.HTTP
{
    public interface IServerResponse
    {
        long StatusCode { get; set; }
        string ErrorMessage { get; set; }
    }


    public class ServerResponse : IServerResponse
    {
        public long StatusCode { get; set; } = -1;

        [JsonProperty(PropertyName = "error")]
        public string ErrorMessage { get; set; } = string.Empty; // Pulled from either the return response, or set manually
    };
}
