using SimpleJSON;

namespace GM.Data
{
    public class GMData
    {
        public GM.Artefacts.Data.ArtefactsData Arts;
        public GM.Mercs.Data.MercsData Mercs;
        public GM.Items.Data.GameItemsData GameItems; // Untested
        public GM.Armoury.Data.ArmouryData Armoury;

        public GMData(JSONNode userJSON, JSONNode gameJSON)
        {
            Arts = new GM.Artefacts.Data.ArtefactsData(userJSON["artefacts"], gameJSON["artefactResources"]);
            Mercs = new GM.Mercs.Data.MercsData(gameJSON["mercResources"]);
            Armoury = new GM.Armoury.Data.ArmouryData(userJSON["armoury"], gameJSON["armouryResources"]);
            GameItems = new GM.Items.Data.GameItemsData();
        }
    }
}
