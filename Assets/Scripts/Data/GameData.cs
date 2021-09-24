using SimpleJSON;
using System;
using UnityEngine;

namespace GM
{
    using GM.Bounties;

    public class GameData
    {
        public static string SERVER_FILE = "game_data";

        static GameData Instance = null;

        public GameBountyData Bounties;
        public DateTime NextDailyReset;

        GameData(JSONNode node)
        {
            Bounties = new GameBountyData(node["bounties"]);

            NextDailyReset = Utils.UnixToDateTime(node["nextDailyReset"].AsLong);
        }


        // = = = Static Methods = = = //
        public static void CreateInstance(JSONNode node)
        {
            Instance = new GameData(node);

            Debug.Log("Created: GameData");
        }


        // = = = Public = = = //
        public static GameData Get => Instance;
        // = = = ^
    }
}