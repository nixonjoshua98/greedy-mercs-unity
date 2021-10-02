using SimpleJSON;
using System;
using UnityEngine.Events;

namespace GM.Core
{
    public class GMData : GMClass
    {
        public Mercs.Data.MercsData Mercs;
        public Armoury.Data.ArmouryData Armoury;
        public Inventory.Data.UserInventory Inv;
        public Artefacts.Data.ArtefactsData Arts;
        public Items.Data.GameItemCollection Items;
        public Bounties.Data.BountiesData Bounties;
        public BountyShop.Data.BountyShopDataCollection BountyShop;

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
            BountyShop = new BountyShop.Data.BountyShopDataCollection(userJSON["bountyShop"]);
        }

        public void Prestige(JSONNode node, UnityAction<bool, JSONNode> callback)
        {
            App.HTTP.Post("prestige", node, (code, resp) => {

                if (code == 200)
                {
                    FileUtils.WriteJSON(FileUtils.ResolvePath("not_game_data"), resp["userData"]);
                }

                callback.Invoke(code == 200, resp);
            });
        }
    }
}