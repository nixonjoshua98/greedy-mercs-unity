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
using UnityEngine;
using System.IO;

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
            Mercs       = new MercsData(userData, staticData.Mercs);
            Artefacts   = new ArtefactsData(userData.Artefacts, staticData.Artefacts);
            Armoury     = new ArmouryData(userData.ArmouryItems, staticData.Armoury);
            Bounties    = new BountiesData(userData.BountyData, staticData.Bounties);
            BountyShop  = new BountyShopData(userData.BountyShop);
        }

        public void ResetPrestigeData()
        {
            GameState = new GameState();

            Mercs.ResetLevels();
            Upgrades.ResetLevels();
            Inv.ResetLocalResources();
        }

        public void Update(IServerUserData userData, IStaticGameData staticData)
        {
            // = Object Recreation = //
            Mercs       = new MercsData(userData, staticData.Mercs);
            BountyShop  = new BountyShopData(userData.BountyShop);

            Inv.UpdateCurrencies(userData.CurrencyItems);
            Artefacts.UpdateAllData(userData.Artefacts, staticData.Artefacts);
            Armoury.UpdateAllData(userData.ArmouryItems, staticData.Armoury);
            Bounties.UpdateAllData(userData.BountyData, staticData.Bounties);
        }
    }
}