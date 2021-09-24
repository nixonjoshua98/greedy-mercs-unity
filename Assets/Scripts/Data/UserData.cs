
using UnityEngine;
using UnityEngine.Events;

using SimpleJSON;

namespace GM
{
    using GM.HTTP;
    using GM.Data;

    using GM.Bounties;

    public class UserData
    {
        public static string SERVER_FILE = "not_game_data";

        static UserData Instance = null;

        public UserBounties Bounties;
        public UserInventory Inventory;
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
            Bounties    = new UserBounties(json["userBountyData"]);
            BountyShop  = new UserBountyShop(json["bountyShop"]);
            Inventory   = new UserInventory(json["inventory"]);
        }


        public void Prestige(JSONNode node, UnityAction<bool, JSONNode> callback)
        {
            LocalDataSaveManager.Get.ClearLocalData(pause: true);

            HTTPClient.Instance.Post("prestige", node, (code, resp) => {

                if (code == 200)
                {
                    FileUtils.WriteJSON(FileUtils.ResolvePath(SERVER_FILE), resp["userData"]);
                }
                else
                {
                    LocalDataSaveManager.Get.Continue();
                }

                callback.Invoke(code == 200, resp);
            });
        }
    }
}