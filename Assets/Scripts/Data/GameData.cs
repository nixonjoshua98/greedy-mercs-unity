using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;

namespace GM.Data
{
    public class GameData
    {
        static GameData Instance = null;

        public GameMercData Mercs;
        public GameBountyData Bounties;

        GameData(JSONNode node)
        {
            Mercs       = new GameMercData(node["mercResources"]);
            Bounties    = new GameBountyData(node["bounties"]);
        }

        // = = = Static Methods = = = //
        public static void CreateInstance(JSONNode node)
        {
            Instance = new GameData(node);

            Debug.Log("Created: GameData");
        }


        public static GameData Get()
        {
            return Instance;
        }


        // = = = ^
    }
}
