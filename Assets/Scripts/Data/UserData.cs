
using UnityEngine;

using SimpleJSON;

namespace GM
{
    using GM.Bounty;
    using GM.Armoury;
    using GM.BountyShop;

    public class UserData
    {
        static UserData Instance = null;

        public BountyManager Bounties;
        public ArmouryManager Armoury;
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
            Armoury     = new ArmouryManager(json["armoury"]);
            BountyShop  = new BountyShopManager(json["bountyShop"]);
        }
    }
}