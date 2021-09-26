using SimpleJSON;

namespace GM.Data
{
    public class GMData
    {
        public Mercs.Data.MercsData Mercs;
        public Armoury.Data.ArmouryData Armoury;
        public Inventory.Data.UserInventory Inv;
        public Artefacts.Data.ArtefactsData Arts;
        public Items.Data.GameItemsData GameItems;

        public GMData(JSONNode userJSON, JSONNode gameJSON)
        {
            Arts = new Artefacts.Data.ArtefactsData(userJSON["artefacts"], gameJSON["artefactResources"]);
            Inv = new Inventory.Data.UserInventory(userJSON["inventory"]);
            Mercs = new Mercs.Data.MercsData(gameJSON["mercResources"]);
            Armoury = new Armoury.Data.ArmouryData(userJSON["armoury"], gameJSON["armouryResources"]);
            GameItems = new Items.Data.GameItemsData();
        }
    }
}