namespace GM.Core
{
    public class GMApplication
    {
        public static GMApplication Instance { get; private set; }

        public GMData Data;
        public GMCache Cache;

        public HTTP.HTTPClient HTTP => GM.HTTP.HTTPClient.Instance;

        public static GMApplication Create(Common.IServerUserData userData, Common.IServerGameData gameData)
        {
            Instance = new GMApplication(userData, gameData);

            return Instance;
        }

        GMApplication(Common.IServerUserData userData, Common.IServerGameData gameData)
        {
            Data = new GMData(userData, gameData);
            Cache = new GMCache();
        }
    }
}