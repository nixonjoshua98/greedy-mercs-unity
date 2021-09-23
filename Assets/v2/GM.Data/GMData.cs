using SimpleJSON;

namespace GM.Data
{
    public class GMData
    {
        public GM.Artefacts.Data.ArtefactsData Arts;
        public GM.Mercs.Data.MercsData Mercs;

        public GMData(JSONNode userJSON, JSONNode gameJSON)
        {
            Arts = new GM.Artefacts.Data.ArtefactsData(userJSON["artefacts"], gameJSON["artefactResources"]);
            Mercs = new GM.Mercs.Data.MercsData(gameJSON["mercResources"]);
        }
    }
}
