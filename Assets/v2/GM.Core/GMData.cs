using System;
using UnityEngine.Events;

namespace GM.Core
{
    public class GMData : GMClass
    {
        public Mercs.Data.MercsData Mercs;
        public Armoury.Data.ArmouryDataCollection Armoury;
        public Inventory.Data.UserInventoryCollection Inv;
        public Artefacts.Data.ArtefactsData Arts;
        public CurrencyItems.Data.CurrencyItemsDataCollection Items;
        public Bounties.Data.BountiesData Bounties;
        public BountyShop.Data.BountyShopData BountyShop;

        public DateTime NextDailyReset;
        public TimeSpan TimeUntilDailyReset => NextDailyReset - DateTime.UtcNow;

        public GMData(Common.Data.IServerUserData userData, Common.Data.IServerGameData gameData)
        {
            Items = new CurrencyItems.Data.CurrencyItemsDataCollection();

            NextDailyReset = gameData.NextDailyReset;

            Inv = new Inventory.Data.UserInventoryCollection(userData.CurrencyItems);
            Mercs = new Mercs.Data.MercsData(gameData.Mercs);
            Arts = new Artefacts.Data.ArtefactsData(userData.Artefacts, gameData.Artefacts);
            Armoury = new Armoury.Data.ArmouryDataCollection(userData.ArmouryItems, gameData.Armoury);
            Bounties = new Bounties.Data.BountiesData(userData.BountyData, gameData.Bounties);
            BountyShop = new BountyShop.Data.BountyShopData(userData.BountyShop);
        }

        public void Prestige(UnityAction<bool> callback)
        {
            App.HTTP.Prestige((resp) =>
            {
                callback.Invoke(resp.StatusCode == 200);
            });
        }
    }
}