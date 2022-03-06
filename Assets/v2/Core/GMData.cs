using GM.Armoury.Data;
using GM.Artefacts.Data;
using GM.Bounties.Data;
using GM.BountyShop.Data;
using GM.Common.Interfaces;
using GM.CurrencyItems.Data;
using GM.Inventory.Data;
using GM.Mercs.Data;
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

        public CurrentPrestigeState GameState;

        public DateTime NextDailyReset;

        public GMData()
        {
            Items = new ItemsData();
            Upgrades = new PlayerUpgrades();
        }

        public void Set(IServerUserData serverUserData, IStaticGameData staticData, LocalStateFile localStateFile)
        {
            NextDailyReset = staticData.NextDailyReset;

            Mercs = new MercsData(serverUserData, staticData, localStateFile);

            GameState = CurrentPrestigeState.Deserialize(localStateFile);

            Inv = new UserInventory(serverUserData.CurrencyItems);
            Artefacts = new ArtefactsData(serverUserData.Artefacts, staticData.Artefacts);
            Armoury = new ArmouryData(serverUserData.ArmouryItems, staticData.Armoury);
            Bounties = new BountiesData(serverUserData.BountyData, staticData.Bounties);
            BountyShop = new BountyShopData(serverUserData.BountyShop);
        }

        public LocalStateFile CreateLocalSaveFile()
        {
            LocalStateFile savefile = new LocalStateFile();

            GameState.UpdateLocalSaveFile(ref savefile);
            Mercs.UpdateLocalSaveFile(ref savefile);

            return savefile;
        }

        public void DeleteLocalStateData()
        {
            GameState = new CurrentPrestigeState();

            Mercs.DeleteLocalStateData();

            Upgrades.DeleteLocalStateData();
            Inv.DeleteLocalStateData();
        }

        public void Update(IServerUserData userData, IStaticGameData staticData)
        {
            Mercs.Update(userData, staticData, null);

            BountyShop  = new BountyShopData(userData.BountyShop);

            Inv.UpdateCurrencies(userData.CurrencyItems);
            Artefacts.UpdateAllData(userData.Artefacts, staticData.Artefacts);
            Armoury.UpdateAllData(userData.ArmouryItems, staticData.Armoury);
            Bounties.UpdateAllData(userData.BountyData, staticData.Bounties);
        }
    }
}