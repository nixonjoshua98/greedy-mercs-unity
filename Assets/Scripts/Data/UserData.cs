
using UnityEngine;

using SimpleJSON;

namespace GM
{
    using GM.Data;

    using GM.Bounty;
    using GM.BountyShop;

    public class UserData
    {
        static UserData Instance = null;

        public BountyManager Bounties;
        public UserArmouryData Armoury;
        public BountyShopManager BountyShop;

        // = = = Static Methods = = = //
        public static void CreateInstance()
        {
            Instance = new UserData();

            Debug.Log("Created: UserData");
        }

        public static UserData Get()
        {
            return Instance;
        }

        // = = = Public Methods = = = //
        public void UpdateWithServerUserData(JSONNode json)
        {
            Bounties    = new BountyManager(json["bounties"]);
            Armoury     = new UserArmouryData(json["armoury"]);
            BountyShop  = new BountyShopManager(json["bountyShop"]);
        }
    }
}