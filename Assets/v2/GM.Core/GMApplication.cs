using SimpleJSON;

namespace GM.Core
{
    public class GMApplication
    {
        public static GMApplication Instance { get; private set; }

        public UserData PlayerData => UserData.Get;
        public GameData GameData => GameData.Get;

        public Data.GMData Data;
        public HTTP.HTTPClient HTTP => GM.HTTP.HTTPClient.Instance;


        public static void Create(JSONNode userJSON, JSONNode gameJSON)
        {
            Instance = new GMApplication(userJSON, gameJSON);
        }


        GMApplication(JSONNode userJSON, JSONNode gameJSON)
        {
            Data = new Data.GMData(userJSON, gameJSON);
        }
    }
}