
using UnityEngine;

using SimpleJSON;

namespace GM
{
    using GM.Data;

    using GM.Bounties;
    using GM.BountyShop;

    public class UserData
    {
        static UserData Instance = null;

        public UserArmoury Armoury;
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

        public static UserData Get()
        {
            return Instance;
        }

        // = = = Public Methods = = = //
        public void UpdateWithServerUserData(JSONNode json)
        {

            Bounties    = new UserBounties(json["bounties"]);
            Armoury     = new UserArmoury(json["armoury"]);
            BountyShop  = new UserBountyShop(json["bountyShop"]);
            Inventory   = new UserInventory(json["inventory"]);
        }
    }
}