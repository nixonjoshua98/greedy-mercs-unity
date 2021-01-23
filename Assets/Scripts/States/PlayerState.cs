using System.Numerics;

using SimpleJSON;

using UnityEngine;


namespace GreedyMercs
{
    [System.Serializable]
    public class PlayerState
    {
        public string username = "Rogue Mercenary";

        public double currentEnergy = 0;
        public int maxPrestigeStage = 0;

        // Currencies
        public BigInteger prestigePoints = 0;
        public long bountyPoints = 0;
        public long weaponPoints = 0;
        public BigDouble gold = 0;
        public long gems = 0;

        public PlayerState(JSONNode node)
        {
            Update(node);
        }

        public void Update(JSONNode node)
        {
            gold = node.HasKey("gold") ? BigDouble.Parse(node["gold"].Value) : gold;
            gems = node.HasKey("gems") ? node["gems"].AsLong : gems;

            maxPrestigeStage    = node.HasKey("maxPrestigeStage") ? node["maxPrestigeStage"].AsInt : maxPrestigeStage;
            currentEnergy       = node.HasKey("currentEnergy") ? node["currentEnergy"].AsFloat : currentEnergy;

            bountyPoints = node.HasKey("bountyPoints") ? node["bountyPoints"].AsLong : bountyPoints;
            weaponPoints = node.HasKey("weaponPoints") ? node["weaponPoints"].AsLong : weaponPoints;

            prestigePoints  = node.HasKey("prestigePoints") ? BigInteger.Parse(node["prestigePoints"].Value, System.Globalization.NumberStyles.Any) : prestigePoints;
        }

        public JSONNode ToJson()
        {
            JSONNode node = JSON.Parse(JsonUtility.ToJson(this));

            node.Add("gold", gold.ToString().Replace("E", "e"));
            node.Add("bountyPoints", bountyPoints.ToString());
            node.Add("prestigePoints", prestigePoints.ToString());

            return node;
        }
    }
}