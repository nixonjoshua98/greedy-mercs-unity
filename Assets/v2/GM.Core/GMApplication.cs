namespace GM.Core
{
    public class GMApplication
    {
        public static GMApplication Instance { get; private set; }

        public GMData Data;

        public HTTP.HTTPClient HTTP => GM.HTTP.HTTPClient.Instance;

        public static GMApplication Create(Common.IServerUserData userData, Common.ICompleteGameData gameData)
        {
            Instance = new GMApplication(userData, gameData);

            return Instance;
        }

        GMApplication(Common.IServerUserData userData, Common.ICompleteGameData gameData)
        {
            Data = new GMData(userData, gameData);
        }
    }
}