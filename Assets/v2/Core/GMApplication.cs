using GM.Common.Interfaces;
using GM.LocalSave;

namespace GM.Core
{
    public class GMApplication
    {
        public static GMApplication Instance { get; private set; }

        public GMData Data;
        public GMCache Cache;
        public LocalSaveManager SaveManager;

        public EventHandler Events = new EventHandler();
        public HTTP.HTTPClient HTTP => GM.HTTP.HTTPClient.Instance;

        public static GMApplication Create(IServerUserData userData, IStaticGameData gameData)
        {
            if (Instance == null)
            {
                Instance = new GMApplication(userData, gameData);
            }

            return Instance;
        }

        GMApplication(IServerUserData userData, IStaticGameData gameData)
        {
            Cache       = new GMCache();
            SaveManager = LocalSaveManager.Create();
            Data        = new GMData(userData, gameData, SaveManager.LoadSaveFile());
        }
    }
}