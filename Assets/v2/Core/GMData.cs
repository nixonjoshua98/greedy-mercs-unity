using System;
using UnityEngine.Events;

namespace GM.Core
{
    public class GMData : GMClass
    {
        public Mercs.Data.MercsData Mercs;
        public Armoury.Data.Armoury Armoury;
        public Inventory.Data.Inventory Inv;
        public Artefacts.Data.Artefacts Artefacts;
        public CurrencyItems.Data.CurrencyItems Items;
        public Bounties.Data.BountiesData Bounties;
        public BountyShop.Data.BountyShopData BountyShop;
        public Upgrades.Data.PlayerUpgrades Upgrades;

        public GM.GameState.GameState GameState;

        public DateTime NextDailyReset;
        public TimeSpan TimeUntilDailyReset => NextDailyReset - DateTime.UtcNow;

        public GMData(Common.Data.IServerUserData userData, Common.Data.IStaticGameData gameData)
        {
            Items = new CurrencyItems.Data.CurrencyItems();
            Upgrades = new Upgrades.Data.PlayerUpgrades();

            GameState = new GameState.GameState();

            NextDailyReset = gameData.NextDailyReset;

            Inv = new Inventory.Data.Inventory(userData.CurrencyItems);
            Mercs = new Mercs.Data.MercsData(userData, gameData);
            Artefacts = new Artefacts.Data.Artefacts(userData.Artefacts, gameData.Artefacts);
            Armoury = new Armoury.Data.Armoury(userData.ArmouryItems, gameData.Armoury);
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