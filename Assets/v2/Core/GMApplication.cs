using GM.Common.Enums;
using GM.GoldUpgrades;
using GM.HTTP;
using GM.LocalFiles;
using GM.Models;
using System;
using UnityEngine.Events;

namespace GM.Core
{
    public class GMApplication
    {
        public static GMApplication Instance { get; private set; }

        // Data Containers
        public GM.Quests.QuestsContainer Quests             = new();
        public GM.Mercs.Data.MercDataContainer Mercs                = new();
        public GoldUpgradesContainer GoldUpgrades               = new();
        public GM.CurrencyItems.Data.ItemsData Items        = new();
        public GM.Inventory.Data.UserInventory Inventory    = new();
        public GM.Artefacts.Data.ArtefactsData Artefacts    = new();
        public GM.Armoury.Data.ArmouryData Armoury          = new();
        public GM.Bounties.Data.BountiesData Bounties       = new();
        public GM.BountyShop.Data.BountyShopData BountyShop = new();
        public CurrentPrestigeState GameState;

        public DateTime NextDailyReset;

        public GMCache GMCache      = new GMCache();
        public EventHandler Events  = new EventHandler();

        public LocalSaveManager SaveManager;
        public LocalPersistantFile PersistantLocalFile;

        public IHTTPClient HTTP => HTTPClient.Instance;

        // Global Events
        public UnityEvent<MercID> E_OnMercUnlocked { get; set; } = new UnityEvent<MercID>();

        private GMApplication()
        {
            SaveManager = LocalSaveManager.Instance; // Force lazy load

            LocalPersistantFile.LoadFromFile(out PersistantLocalFile);
        }

        public static GMApplication Create()
        {
            if (Instance == null)
                Instance = new GMApplication();

            return Instance;
        }

        public void UpdateDataContainers(IServerUserData userData, IStaticGameData staticData)
        {
            UpdateDataContainers(userData, staticData, null);
            DeleteLocalStateData();
        }

        public void UpdateDataContainers(IServerUserData userData, IStaticGameData staticData, LocalStateFile stateFile)
        {
            NextDailyReset = staticData.NextDailyReset;

            GameState = CurrentPrestigeState.Deserialize(stateFile);

            BountyShop.Set(userData.BountyShop);
            Quests.Set(staticData.Quests, userData.Quests);
            Mercs.Set(userData.UnlockedMercs, staticData.Mercs, stateFile);
            Armoury.Set(userData.ArmouryItems, staticData.Armoury);
            Bounties.Set(userData.BountyData, staticData.Bounties);
            Inventory.Set(userData.CurrencyItems);
            Artefacts.Set(userData.Artefacts, staticData.Artefacts);
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

            GoldUpgrades.DeleteLocalStateData();
            Inventory.DeleteLocalStateData();
        }

        public void InvalidateClient()
        {
            
        }
    }
}