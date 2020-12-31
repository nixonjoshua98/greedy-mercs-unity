using System;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;

using Characters        = CharacterData.Characters;
using Passives          = PassivesData.Passives;
using Bounties          = BountyData.Bounties;
using Relics            = RelicData.Relics;



public class StaticData
{
    static _StaticData Instance = null;

    // === Accessors ===
    public static Relics Relics { get { return Instance.relics; } }
    public static Passives Passives { get { return Instance.passives; } }
    public static Characters Characters {  get { return Instance.characters; } }

    public static void Restore(JSONNode json)
    {
        if (Instance == null)
        {
            Instance = new _StaticData(json);
        }
    }

    class _StaticData
    {
        // === Attributes ===
        public Characters   characters;
        public Passives     passives;
        public Bounties     bounties;
        public Relics       relics;

        public _StaticData(JSONNode json)
        {
            relics = new Relics(json["relics"]);
            bounties = new Bounties(json["bounties"]);
            passives = new Passives(json["characterPassives"]);
            characters = new Characters(json["characters"]);
        }
    }
}
