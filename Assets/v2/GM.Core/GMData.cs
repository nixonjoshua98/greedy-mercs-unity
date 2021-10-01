using SimpleJSON;
using System;

namespace GM.Core
{
    public class GMData
    {
        public Mercs.Data.MercsData Mercs;
        public Armoury.Data.ArmouryData Armoury;
        public Inventory.Data.UserInventory Inv;
        public Artefacts.Data.ArtefactsData Arts;
        public Items.Data.GameItemCollection Items { get; set; }
        public Bounties.Data.BountiesData Bounties;

        public DateTime NextDailyReset;

        public GMData(JSONNode userJSON, JSONNode gameJSON)
        {
            NextDailyReset = Utils.UnixToDateTime(gameJSON["nextDailyReset"].AsLong);

            Items = new Items.Data.GameItemCollection();

            Inv = new Inventory.Data.UserInventory(userJSON["inventory"]);

            Mercs = new Mercs.Data.MercsData(gameJSON["mercs_gameData"]);

            Arts = new Artefacts.Data.ArtefactsData(userJSON["artefacts_userData"], gameJSON["artefacts_gameData"]);
            Armoury = new Armoury.Data.ArmouryData(userJSON["armoury_userData"], gameJSON["armoury_gameData"]);
            Bounties = new Bounties.Data.BountiesData(userJSON["bounties_userData"], gameJSON["bounties_gameData"]);
        }
    }
}