using System.Collections;
using System.Collections.Generic;

using SimpleJSON;

namespace GM
{
    using GM.Bounty;
    public class UserData
    {
        static UserData Instance = null;

        public BountyManager Bounties;

        // = = = Static Methods = = = //
        public static UserData CreateInstance()
        {
            Instance = new UserData();

            return Instance;
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