using SimpleJSON;
using System;
using UnityEngine.Events;

namespace GM.Core
{
    public class GMData : GMClass
    {
        public Mercs.Data.MercsData Mercs;
        public Armoury.Data.ArmouryDataController Armoury;
        public Inventory.Data.UserInventoryCollection Inv;
        public Artefacts.Data.ArtefactsData Arts;
        public CurrencyItems.Data.CurrencyItemsDataCollection Items;
        public Bounties.Data.BountiesData Bounties;
        public BountyShop.Data.BountyShopDataCollection BountyShop;

        public DateTime NextDailyReset;

        public GMData(JSONNode userJSON, JSONNode gameJSON)
        {
            Items = new CurrencyItems.Data.CurrencyItemsDataCollection(); // Should always be first

            NextDailyReset = Utils.UnixToDateTime(gameJSON["nextDailyReset"].AsLong);

            Inv = new Inventory.Data.UserInventoryCollection(userJSON["inventory"]);

            Mercs = new Mercs.Data.MercsData(gameJSON["mercs_gameData"]);

            Arts = new Artefacts.Data.ArtefactsData(userJSON["artefacts_userData"], gameJSON["artefacts_gameData"]);
            Armoury = new Armoury.Data.ArmouryDataController(userJSON["armoury_userData"], gameJSON["armoury_gameData"]);
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