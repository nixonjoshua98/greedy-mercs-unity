using SimpleJSON;
using UnityEngine;

namespace GM.Core
{
    public class GMApplication
    {
        public static GMApplication Instance { get; private set; }

        public GMData Data;

        public HTTP.HTTPClient HTTP => GM.HTTP.HTTPClient.Instance;


        public static void Create(JSONNode userJSON, JSONNode gameJSON)
        {
            Instance = new GMApplication(userJSON, gameJSON);
        }


        GMApplication(JSONNode userJSON, JSONNode gameJSON)
        {
            Data = new GMData(userJSON, gameJSON);

            Resources.UnloadUnusedAssets();
        }
    }
}