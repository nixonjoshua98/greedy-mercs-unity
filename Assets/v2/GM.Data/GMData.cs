using SimpleJSON;

namespace GM.Data
{
    public class GMData
    {
        public Mercs.Data.MercsData Mercs;
        public Armoury.Data.ArmouryData Armoury;
        public Artefacts.Data.ArtefactsData Arts;
        public Items.Data.GameItemsData GameItems;

        public GMData(JSONNode userJSON, JSONNode gameJSON)
        {
            Arts = new Artefacts.Data.ArtefactsData(userJSON["artefacts"], gameJSON["artefactResources"]);
            Mercs = new Mercs.Data.MercsData(gameJSON["mercResources"]);
            Armoury = new Armoury.Data.ArmouryData(userJSON["armoury"], gameJSON["armouryResources"]);
            GameItems = new Items.Data.GameItemsData();
        }
    }
}