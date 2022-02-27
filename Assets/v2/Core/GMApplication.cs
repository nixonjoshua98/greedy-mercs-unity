using GM.Common.Interfaces;
using GM.LocalSave;

namespace GM.Core
{
    public class GMApplication
    {
        public static GMApplication Instance { get; private set; }

        public AssetBundlesManager AssetBundles;

        public GMData Data;
        public GMCache Cache;
        public LocalSaveManager SaveManager;

        public EventHandler Events = new EventHandler();
        public HTTP.HTTPClient HTTP => GM.HTTP.HTTPClient.Instance;

        public static GMApplication Create(IServerUserData userData, IStaticGameData gameData)
        {
            if (Instance == null)
            {
                Instance = new GMApplication();

                Instance.AssetBundles = new AssetBundlesManager();
                Instance.AssetBundles.Load();

                Instance.Cache = new GMCache();
                Instance.SaveManager = LocalSaveManager.Create();
                Instance.Data = new GMData(userData, gameData, Instance.SaveManager.LoadSaveFile());
            }

            return Instance;
        }
    }
}