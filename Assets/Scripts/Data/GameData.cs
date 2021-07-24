
using UnityEngine;

using SimpleJSON;

namespace GM.Data
{
    using GM.Artefacts;
    public class GameData
    {
        static GameData Instance = null;

        public GameMercData Mercs;
        public GameArmouryData Armoury;
        public GameBountyData Bounties;
        public GameArtefactData Artefacts;


        GameData(JSONNode node)
        {
            Artefacts   = new GameArtefactData(node["artefactResources"]);
            Armoury     = new GameArmouryData(node["armouryResources"]);
            Mercs       = new GameMercData(node["mercResources"]);
            Bounties    = new GameBountyData(node["bounties"]);
        }


        // = = = Static Methods = = = //
        public static void CreateInstance(JSONNode node)
        {
            Instance = new GameData(node);

            Debug.Log("Created: GameData");
        }


        // = = = Public = = = //
        public static GameData Get()
        {
            return Instance;
        }


        // = = = ^
    }
}
