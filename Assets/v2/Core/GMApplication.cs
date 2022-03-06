using GM.Common.Interfaces;
using GM.LocalFiles;
using UnityEngine;

namespace GM.Core
{
    public class GMApplication
    {
        public static GMApplication Instance { get; private set; }

        public GMData GMData;
        public GMCache GMCache;
        public LocalSaveManager SaveManager;
        public PersistantLocalFile PersistantLocalFile;

        public EventHandler Events = new EventHandler();
        public HTTP.HTTPClient HTTP => GM.HTTP.HTTPClient.Instance;

        private GMApplication(IServerUserData userData, IStaticGameData gameData)
        {
            GMLogger.Editor(Application.persistentDataPath);

            GMCache = new GMCache();
            SaveManager = LocalSaveManager.Create();
            GMData = new GMData(userData, gameData, SaveManager.LoadSaveFile());

            LoadPersistantLocalFile();
        }

        public static GMApplication Create(IServerUserData userData, IStaticGameData gameData)
        {
            if (Instance == null)
                Instance = new GMApplication(userData, gameData);

            return Instance;
        }

        void LoadPersistantLocalFile()
        {
            FileStatus status = PersistantLocalFile.LoadFromFile(out PersistantLocalFile);

            GMLogger.Editor($"PersistantLocalFile: {status}");

            GMData.Mercs.ValidatePersistantLocalFile(ref PersistantLocalFile);
        }
    }
}