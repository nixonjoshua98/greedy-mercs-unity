using GM.Armoury.Data;
using GM.Artefacts.Data;
using GM.Bounties.Data;
using GM.BountyShop.Data;
using GM.CurrencyItems.Data;
using GM.Inventory.Data;
using GM.Mercs.Data;
using GM.Models;
using GM.Upgrades.Data;
using System;

namespace GM.Core
{
    public class GMData : GMClass
    {
        public Quests.QuestsDataContainer Quests = new GM.Quests.QuestsDataContainer();

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

        public void Set(IServerUserData userData, IStaticGameData staticData, LocalStateFile stateFile)
        {
            Quests.Set(staticData.Quests, userData.Quests);

            NextDailyReset = staticData.NextDailyReset;

            Mercs = new MercsData(userData, staticData, stateFile);

            GameState = CurrentPrestigeState.Deserialize(stateFile);

            Inv = new UserInventory(userData.CurrencyItems);
            Artefacts = new ArtefactsData(userData.Artefacts, staticData.Artefacts);
            Armoury = new ArmouryData(userData.ArmouryItems, staticData.Armoury);
            Bounties = new BountiesData(userData.BountyData, staticData.Bounties);
            BountyShop = new BountyShopData(userData.BountyShop);
        }

        public void Update(IServerUserData userData, IStaticGameData staticData)
        {
            Quests.Set(staticData.Quests, userData.Quests);

            Mercs.Update(userData, staticData, null);

            BountyShop = new BountyShopData(userData.BountyShop);

            Inv.UpdateCurrencies(userData.CurrencyItems);
            Artefacts.UpdateAllData(userData.Artefacts, staticData.Artefacts);
            Armoury.UpdateAllData(userData.ArmouryItems, staticData.Armoury);
            Bounties.UpdateAllData(userData.BountyData, staticData.Bounties);
        }

        public LocalStateFile CreateLocalStateFile()
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
    }
}