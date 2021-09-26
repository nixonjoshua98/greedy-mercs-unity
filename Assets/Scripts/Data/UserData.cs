
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;

namespace GM
{
    using GM.Bounties;
    using GM.HTTP;

    public class UserData
    {
        public static string SERVER_FILE = "not_game_data";

        static UserData Instance = null;

        public UserBountyShop BountyShop;

        // = = = Static Methods = = = //
        public static UserData CreateInstance()
        {
            Instance = new UserData();

            Debug.Log("Created: UserData");

            return Instance;
        }

        public static UserData Get => Instance;

        // = = = Public Methods = = = //
        public void UpdateWithServerUserData(JSONNode json)
        {
            BountyShop  = new UserBountyShop(json["bountyShop"]);
        }


        public void Prestige(JSONNode node, UnityAction<bool, JSONNode> callback)
        {
            HTTPClient.Instance.Post("prestige", node, (code, resp) => {

                if (code == 200)
                {
                    FileUtils.WriteJSON(FileUtils.ResolvePath(SERVER_FILE), resp["userData"]);
                }

                callback.Invoke(code == 200, resp);
            });
        }
    }
}