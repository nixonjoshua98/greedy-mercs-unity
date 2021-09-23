using SimpleJSON;

using ArtefactsData = GM.Data.Artefacts.ArtefactsData;

namespace GM.Data
{
    public class GMData
    {
        public ArtefactsData Arts;

        public GMData(JSONNode userJSON, JSONNode gameJSON)
        {
            Arts = new ArtefactsData(userJSON["artefacts"], gameJSON["artefactsResources"]);
        }
    }
}
