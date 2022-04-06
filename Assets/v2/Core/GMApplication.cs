using GM.HTTP;
using GM.LocalFiles;
using GM.Models;
using System;

namespace GM.Core
{
    public class GMApplication
    {
        public static GMApplication Instance { get; private set; }

        // Data Containers
        public GM.Quests.QuestsContainer Quests = new();
        public GM.Mercs.Data.MercDataContainer Mercs = new();
        public GM.GoldUpgrades.GoldUpgradesContainer GoldUpgrades = new();
        public GM.CurrencyItems.Data.ItemsData Items = new();
        public GM.Inventory.Data.UserInventory Inventory = new();
        public GM.Artefacts.Data.ArtefactsData Artefacts = new();
        public GM.Armoury.Data.ArmouryDataContainer Armoury = new();
        public GM.Bounties.Models.BountiesDataContainer Bounties = new();
        public GM.BountyShop.Data.BountyShopDataContainer BountyShop = new();
        public GM.PlayerStats.PlayerStatsContainer Stats = new();

        public CurrentPrestigeState GameState;

        public GMCache GMCache = new();
        public EventHandler Events = new();

        public LocalSaveManager SaveManager;
        public LocalPersistantFile PersistantLocalFile;

        public IHTTPClient HTTP => HTTPClient.Instance;

        // Server Refresh
        public ServerRefreshInterval DailyRefresh = new() { Hour = 20, Interval = TimeSpan.FromDays(1) };

        private GMApplication()
        {
            SaveManager = LocalSaveManager.Instance;    // Force lazy load

            LocalPersistantFile.LoadFromFile(out PersistantLocalFile);
        }

        public static GMApplication Create()
        {
            GMLogger.Editor(UnityEngine.Application.persistentDataPath);

            if (Instance == null)
                Instance = new GMApplication();

            return Instance;
        }

        public void UpdateDataContainers(IServerUserData userData, IStaticGameData staticData, LocalStateFile stateFile = null)
        {
            // Property has a date check, so if the data is old then it will recreate a new instance
            PersistantLocalFile.LocalDailyStats.DateTime = DateTime.UtcNow;

            GameState = stateFile == null ? new() : stateFile.GameState;

            Stats.Set(userData.LifetimeStats);
            BountyShop.Set(userData.BountyShop);
            Quests.Set(userData.Quests);
            Mercs.Set(userData.UnlockedMercs, staticData.Mercs, stateFile);
            Armoury.Set(userData.ArmouryItems, staticData.ArmouryItems);
            Bounties.Set(userData.Bounties, staticData.Bounties);
            Inventory.Set(userData.Currencies);
            Artefacts.Set(userData.Artefacts, staticData.Artefacts);
        }

        public void SaveLocalStateFile()
        {
            LocalStateFile savefile = new();

            GameState.UpdateLocalSaveFile(ref savefile);
            Mercs.UpdateLocalSaveFile(ref savefile);

            savefile.WriteToFile();
        }

        public void DeleteLocalStateData()
        {
            GameState = new CurrentPrestigeState();

            Mercs.DeleteLocalStateData();

            GoldUpgrades.DeleteLocalStateData();
            Inventory.DeleteLocalStateData();
        }
    }
}