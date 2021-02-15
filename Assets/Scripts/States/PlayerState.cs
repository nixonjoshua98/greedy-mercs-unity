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

        public BigDouble gold  = 0;

        public PlayerState(JSONNode node)
        {
            Update(node);
        }

        public void Reset()
        {
            gold            = 0;
            currentEnergy   = 0;
        }

        public void Update(JSONNode node)
        {
            gold = node.HasKey("gold") ? BigDouble.Parse(node["gold"].Value) : gold;

            username        = node.HasKey("username") ? node["username"].Value : username;
            currentEnergy   = node.HasKey("currentEnergy") ? node["currentEnergy"].AsFloat : currentEnergy;
        }

        public JSONNode ToJson()
        {
            JSONNode node = JSON.Parse(JsonUtility.ToJson(this));

            node.Add("gold", gold.ToString().Replace("E", "e"));

            return node;
        }
    }
}