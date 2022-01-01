using GM.Common.Models;

namespace GM.HTTP.Requests
{
    public class PrestigeRequest : IServerRequest
    {

    }

    public class PrestigeResponse : ServerResponse
    {
        public StaticGameDataModel StaticData { get; set; }
        public ServerUserDataModel UserData { get; set; }
    }
}
