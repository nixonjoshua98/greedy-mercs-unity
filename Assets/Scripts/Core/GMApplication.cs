using GM.HTTP;
using GM.LocalFiles;
using GM.Models;
using GM.ScriptableObjects;
using System;
using UnityEngine;

namespace GM.Core
{
    public class GMApplication : UnityEngine.MonoBehaviour
    {
        public static GMApplication Instance { get; private set; }

        // Data Containers
        public GM.Quests.QuestsContainer Quests = new();
        public GM.Mercs.Data.MercDataContainer Mercs = new();
        public GM.GoldUpgrades.GoldUpgradesContainer GoldUpgrades = new();
        public GM.Inventory.Data.UserInventory Inventory = new();
        public GM.Artefacts.Data.ArtefactsDataContainer Artefacts = new();
        public GM.Armoury.Data.ArmouryDataContainer Armoury = new();
        public GM.Bounties.Models.BountiesDataContainer Bounties = new();
        public GM.BountyShop.Data.BountyShopDataContainer BountyShop = new();
        public GM.UserStats.PlayerStatsContainer Stats = new();

        public LocalDataContainer Local;

        [Tooltip("Local Save Manager Singleton")]
        public LocalSaveManager SaveManager;

        /* Current state - Forward it from the state file */
        public GameState GameState { get => LocalStateFile.GameState; }

        public GMValues Values = new();

        /* Local Files */
        public LocalPersistantFile PersistantLocalFile;
        public LocalStateFile LocalStateFile;

        /// <summary>
        /// Main HTTP client for communicating with the server
        /// </summary>
        public IHTTPClient HTTP => HTTPClient.Instance;

        /// <summary>
        /// Daily refresh config (ensures data is valid within the refresh period)
        /// </summary>
        public ServerRefreshInterval DailyRefresh = new() { Hour = 20, Interval = TimeSpan.FromDays(1) };

        void Awake()
        {
            Instance = this;

            DontDestroyOnLoad(this);
        }

        public void Initialize()
        {
            GMLogger.Editor(Application.persistentDataPath);

            LocalPersistantFile.LoadFromFile(out PersistantLocalFile);
            LocalStateFile.LoadFromFile(out LocalStateFile);
        }

        public void UpdateDataContainers(IServerUserData userData, IStaticGameData staticData)
        {
            // Property has a date check, so if the data is old then it will recreate a new instance
            PersistantLocalFile.LocalDailyStats.LastUpdated = DateTime.UtcNow;

            Stats.Set(userData.LifetimeStats);
            Quests.Set(userData.Quests);
            Mercs.Set(userData.UnlockedMercs, staticData.Mercs);
            Armoury.Set(userData.ArmouryItems, staticData.ArmouryItems);
            Bounties.Set(userData.Bounties, staticData.Bounties);
            Inventory.Set(userData.Currencies);
            Artefacts.Set(userData.Artefacts, staticData.Artefacts);
        }

        public void DeleteLocalStateData()
        {
            GoldUpgrades.DeleteLocalStateData();
        }
    }
}