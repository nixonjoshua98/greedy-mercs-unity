using GM.Armoury.Data;
using GM.Artefacts.Data;
using GM.Bounties.Data;
using GM.BountyShop.Data;
using GM.Common.Interfaces;
using GM.CurrencyItems.Data;
using GM.Inventory.Data;
using GM.LocalSave;
using GM.Mercs.Data;
using GM.States;
using GM.Upgrades.Data;
using System;

namespace GM.Core
{
    public class GMData : GMClass
    {
        public MercsData Mercs;
        public ArmouryData Armoury;
        public UserInventory Inv;
        public ArtefactsData Artefacts;
        public ItemsData Items;
        public BountiesData Bounties;
        public BountyShopData BountyShop;
        public PlayerUpgrades Upgrades;

        public GameState GameState;

        public DateTime NextDailyReset;
        public TimeSpan TimeUntilDailyReset => NextDailyReset - DateTime.UtcNow;

        public GMData(IServerUserData userData, IStaticGameData staticData, LocalSaveFileModel localSaveFile)
        {
            NextDailyReset = staticData.NextDailyReset;

            // = Scriptable Object Models = //
            Items = new ItemsData();

            // = Local Models = //
            GameState = GameState.Deserialize(localSaveFile);

            Upgrades    = new PlayerUpgrades();
            Inv         = new UserInventory(userData.CurrencyItems);
            Mercs       = new MercsData(userData, staticData);
            Artefacts   = new ArtefactsData(userData.Artefacts, staticData.Artefacts);
            Armoury     = new ArmouryData(userData.ArmouryItems, staticData.Armoury);
            Bounties    = new BountiesData(userData.BountyData, staticData.Bounties);
            BountyShop  = new BountyShopData(userData.BountyShop);
        }

        public void ResetPrestigeData()
        {
            Mercs.ResetLevels();
            Upgrades.ResetLevels();
            Inv.ResetLocalResources();
        }

        public void Update(IServerUserData userData, IStaticGameData staticData)
        {
            Inv.UpdateCurrencies(userData.CurrencyItems);
            Mercs.UpdateAllData(userData.UnlockedMercs, staticData.Mercs);
            Artefacts.UpdateAllData(userData.Artefacts, staticData.Artefacts);
            Armoury.UpdateAllData(userData.ArmouryItems, staticData.Armoury);
            Bounties.UpdateAllData(userData.BountyData, staticData.Bounties);
            BountyShop.UpdateShop(userData.BountyShop);
        }
    }
}