using GM.Common.Enums;
using GM.HTTP;
using GM.LocalFiles;
using UnityEngine.Events;

namespace GM.Core
{
    public class GMApplication
    {
        public static GMApplication Instance { get; private set; }

        public GMData DataContainers;
        public GMCache GMCache;

        public LocalSaveManager SaveManager { get; set; }
        public LocalPersistantFile PersistantLocalFile;

        public EventHandler Events = new EventHandler();
        public IHTTPClient HTTP => HTTPClient.Instance;

        // Global Events
        public UnityEvent<MercID> E_OnMercUnlocked = new UnityEvent<MercID>();

        public GMApplication(LocalPersistantFile persistantLocalFile, GMData dataContainer, LocalSaveManager localSaveManager)
        {
            PersistantLocalFile = persistantLocalFile;
            SaveManager = localSaveManager;

            GMCache = new GMCache();
            DataContainers = dataContainer;
        }

        public void InvalidateClient()
        {
            
        }

        public void SetInstance()
        {
            Instance = this;
        }
    }
}