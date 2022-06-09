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
        public GM.PlayerStats.PlayerStatsContainer Stats = new();

        [Tooltip("Local Data (ScriptableObjects etc.)")]
        public LocalDataContainer Local;

        [Tooltip("Local Save Manager - MonoBehaviour Singleton ")]
        public LocalSaveManager SaveManager;


        public CurrentPrestigeState GameState;
        public GMCache GMCache = new();
        public LocalPersistantFile PersistantLocalFile;

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
        }

        public void UpdateDataContainers(IServerUserData userData, IStaticGameData staticData, LocalStateFile stateFile = null)
        {
            // Property has a date check, so if the data is old then it will recreate a new instance
            PersistantLocalFile.LocalDailyStats.LastUpdated = DateTime.UtcNow;

            GameState = stateFile == null ? new() : stateFile.GameState;

            Stats.Set(userData.LifetimeStats);
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