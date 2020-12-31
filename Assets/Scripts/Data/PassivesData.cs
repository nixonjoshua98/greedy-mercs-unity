using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;

namespace PassivesData
{
    public class PassiveSkill
    {
        public BonusType bonusType;

        public string name;
        public string description;

        public double value;
    }

    public class Passives
    {
        public Dictionary<int, PassiveSkill> passives;

        public Passives(JSONNode node)
        {
            passives = new Dictionary<int, PassiveSkill>();

            foreach (string key in node.Keys)
            {
                passives[int.Parse(key)] = JsonUtility.FromJson<PassiveSkill>(node[key].ToString());
            }
        }

        public PassiveSkill Get(int id)
        {
            return passives[id];
        }
    }
}
