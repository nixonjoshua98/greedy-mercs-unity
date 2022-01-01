using GM.Common.Models;

namespace GM.HTTP.Requests
{
    public class FetchGameDataResponse : ServerResponse
    {
        public StaticGameDataModel StaticData { get; set; }
    }
}
