using SimpleJSON;
using System;

namespace GM.Data
{
    public class GMData
    {
        public Mercs.Data.MercsData Mercs;
        public Armoury.Data.ArmouryData Armoury;
        public Inventory.Data.UserInventory Inv;
        public Artefacts.Data.ArtefactsData Arts;
        public Items.Data.ItemsData Items;
        public Bounties.Data.BountiesData Bounties;

        public DateTime NextDailyReset;

        public GMData(JSONNode userJSON, JSONNode gameJSON)
        {
            NextDailyReset = Utils.UnixToDateTime(gameJSON["nextDailyReset"].AsLong);

            Inv = new Inventory.Data.UserInventory(userJSON["inventory"]);
            Arts = new Artefacts.Data.ArtefactsData(userJSON["artefacts"], gameJSON["artefactResources"]);
            Items = new Items.Data.ItemsData();
            Mercs = new Mercs.Data.MercsData(gameJSON["mercResources"]);
            Armoury = new Armoury.Data.ArmouryData(userJSON["armoury"], gameJSON["armouryResources"]);
            Bounties = new Bounties.Data.BountiesData(gameJSON["bounties_gameData"]);
        }
    }
}