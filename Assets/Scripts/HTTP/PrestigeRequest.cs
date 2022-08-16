using SRC.Models;

namespace SRC.HTTP.Requests
{
    public class PrestigeResponse : ServerResponse
    {
        public StaticGameDataModel DataFiles { get; set; }
        public ServerUserDataModel UserData { get; set; }
    }
}
