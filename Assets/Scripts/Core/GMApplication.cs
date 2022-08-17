using SRC.HTTP;
using SRC.LocalFiles;
using SRC.Models;
using SRC.ScriptableObjects;
using System;
using UnityEngine;

namespace SRC.Core
{
    [System.Serializable]
    public class GMApplication
    {
        public static GMApplication Instance { get; private set; }

        // Data Containers
        public SRC.Quests.QuestsContainer Quests = new();
        public SRC.Mercs.Data.MercDataContainer Mercs = new();
        public SRC.GoldUpgrades.GoldUpgradesContainer GoldUpgrades = new();
        public SRC.Inventory.Data.UserInventory Inventory = new();
        public SRC.Artefacts.Data.ArtefactsDataContainer Artefacts = new();
        public SRC.Armoury.Data.ArmouryDataContainer Armoury = new();
        public SRC.Bounties.Models.BountiesDataContainer Bounties = new();
        public SRC.BountyShop.Data.BountyShopDataContainer BountyShop = new();
        public SRC.UserStats.PlayerStatsContainer Stats = new();

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

        public GMApplication()
        {
            Instance = this;
        }

        public void Initialize(IServerUserData userData, IStaticGameData staticData)
        {
            GMLogger.Editor(Application.persistentDataPath);

            LocalPersistantFile.LoadFromFile(out PersistantLocalFile);
            LocalStateFile.LoadFromFile(out LocalStateFile);

            UpdateDataContainers(userData, staticData);
        }

        public void UpdateDataContainers(IServerUserData userData, IStaticGameData staticData)
        {
            // Property has a date check, so if the data is old then it will recreate a new instance
            PersistantLocalFile.LocalDailyStats.LastUpdated = DateTime.UtcNow;

            Stats.Set(userData.LifetimeStats);
            Quests.Set(userData.Quests);
            Mercs.Set(userData.UnlockedMercs, staticData.Mercs);
            Armoury.Set(userData.ArmouryItems, staticData.ArmouryItems);
            Bounties.UpdateStoredData(userData.Bounties, staticData.Bounties);
            Inventory.Set(userData.Currencies);
            Artefacts.Set(userData.Artefacts, staticData.Artefacts);
        }

        public void DeleteLocalStateData()
        {
            GoldUpgrades.DeleteLocalStateData();
        }
    }
}