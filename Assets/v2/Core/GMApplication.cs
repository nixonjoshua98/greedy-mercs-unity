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
        public GM.Armoury.Data.ArmouryData Armoury = new();
        public GM.Bounties.Data.BountiesData Bounties = new();
        public GM.BountyShop.Data.BountyShopData BountyShop = new();
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
            PersistantLocalFile.LocalDailyStats.CreatedTime = DailyRefresh.Next;

            GameState = stateFile == null ? new() : stateFile.GameState;

            Stats.Set(userData.UserStats);
            BountyShop.Set(userData.BountyShop);
            Quests.Set(userData.Quests);
            Mercs.Set(userData.UnlockedMercs, staticData.Mercs, stateFile);
            Armoury.Set(userData.ArmouryItems, staticData.Armoury);
            Bounties.Set(userData.BountyData, staticData.Bounties);
            Inventory.Set(userData.CurrencyItems);
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

        public void InvalidateClient()
        {

        }
    }
}