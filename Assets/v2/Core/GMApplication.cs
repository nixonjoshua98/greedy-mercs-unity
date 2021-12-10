namespace GM.Core
{
    public class GMApplication
    {
        public static GMApplication Instance { get; private set; }

        public GMData Data;
        public GMCache Cache;

        public GenericEvents Events = new GenericEvents();
        public HTTP.HTTPClient HTTP => GM.HTTP.HTTPClient.Instance;

        public static GMApplication Create(Common.Data.IServerUserData userData, Common.Data.IServerGameData gameData)
        {
            Instance = new GMApplication(userData, gameData);

            return Instance;
        }

        GMApplication(Common.Data.IServerUserData userData, Common.Data.IServerGameData gameData)
        {
            Cache = new GMCache();

            Data = new GMData(userData, gameData);
        }
    }
}