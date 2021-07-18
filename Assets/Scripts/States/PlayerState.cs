using System.Numerics;

using SimpleJSON;

using UnityEngine;


namespace GM
{
    [System.Serializable]
    public class PlayerState
    {
        public double currentEnergy;

        public BigDouble gold;

        public PlayerState()
        {
            Reset();
        }

        public void Reset()
        {
            gold            = 0;
            currentEnergy   = 0;
        }


        public void Update(JSONNode node)
        {
            gold = node.HasKey("gold") ? BigDouble.Parse(node["gold"].Value) : gold;

            currentEnergy   = node.HasKey("currentEnergy") ? node["currentEnergy"].AsFloat : currentEnergy;
        }
    }
}