
using UnityEngine;
using UnityEngine.Events;

using SimpleJSON;

namespace GM
{
    using GM.Server;
    using GM.Data;

    using GM.Bounty;
    using GM.Artefacts;

    public class UserData
    {
        static UserData Instance = null;

        public UserArmoury Armoury;
        public UserBounties Bounties;
        public UserArtefacts Artefacts;
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

            Bounties    = new UserBounties(json["bounties"]);
            Armoury     = new UserArmoury(json["armoury"]);
            BountyShop  = new UserBountyShop(json["bountyShop"]);
            Artefacts   = new UserArtefacts(json["artefacts"]);
            Inventory   = new UserInventory(json["inventory"]);
        }


        public void Prestige(JSONNode node, UnityAction<bool, JSONNode> callback)
        {
            LocalDataSaveManager.Get.ClearLocalData(pause: true);

            HTTPClient.GetClient().Post("prestige", node, (code, resp) => {

                if (code == 200)
                {
                    UpdateWithServerUserData(resp["completeUserData"]);
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