using GM.Common.Interfaces;

namespace GM.Core
{
    public class GMApplication
    {
        public static GMApplication Instance { get; private set; }

        public GMData GMData;
        public GMCache GMCache;
        public LocalSaveManager SaveManager;

        public EventHandler Events = new EventHandler();
        public HTTP.HTTPClient HTTP => GM.HTTP.HTTPClient.Instance;

        public static GMApplication Create(IServerUserData userData, IStaticGameData gameData)
        {
            if (Instance == null)
            {
                Instance = new GMApplication();
                Instance.GMCache = new GMCache();

                Instance.SaveManager = LocalSaveManager.Create();
                Instance.GMData = new GMData(userData, gameData, Instance.SaveManager.LoadSaveFile());
            }

            return Instance;
        }
    }
}