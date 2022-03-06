using GM.LocalFiles;

namespace GM.Core
{
    public class GMApplication
    {
        public static GMApplication Instance { get; private set; }

        public GMData GMData;
        public GMCache GMCache;

        public LocalSaveManager SaveManager { get; set; }
        public LocalPersistantFile PersistantLocalFile;

        public EventHandler Events = new EventHandler();
        public HTTP.HTTPClient HTTP => GM.HTTP.HTTPClient.Instance;

        public GMApplication(LocalPersistantFile persistantLocalFile, GMData dataContainer, LocalSaveManager localSaveManager)
        {
            PersistantLocalFile = persistantLocalFile;
            SaveManager = localSaveManager;

            GMCache = new GMCache();
            GMData = dataContainer;
        }

        public void SetInstance()
        {
            Instance = this;
        }
    }
}