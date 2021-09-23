using System;
using UnityEngine;

using SimpleJSON;

namespace GM
{
    using GM.Data;
    using GM.Bounties;

    public class GameData
    {
        public static string SERVER_FILE = "game_data";

        static GameData Instance = null;

        public GameItemData Items;
        public GameMercData Mercs;
        public GameArmouryData Armoury;
        public GameBountyData Bounties;
        public DateTime NextDailyReset;

        GameData(JSONNode node)
        {
            Items = new GameItemData();

            Armoury     = new GameArmouryData(node["armouryResources"]);
            Mercs       = new GameMercData(node["mercResources"]);
            Bounties    = new GameBountyData(node["bounties"]);

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
