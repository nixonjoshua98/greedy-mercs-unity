
using UnityEngine;

using SimpleJSON;

namespace GM
{
    using GM.Bounty;
    public class UserData
    {
        static UserData Instance = null;

        public BountyManager Bounties;

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
            Bounties = new BountyManager(json["bounties"]);
        }
    }
}