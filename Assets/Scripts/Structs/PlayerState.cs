using System.Numerics;

using SimpleJSON;

using UnityEngine;


[System.Serializable]
public class PlayerState
{
    public double gold;

    public BigInteger prestigePoints;

    public void Update(JSONNode node)
    {
        prestigePoints = node.HasKey("prestigePoints") ? BigInteger.Parse(node["prestigePoints"], System.Globalization.NumberStyles.Any) : 0;
    }

    public JSONNode ToJson()
    {
        JSONNode node = JSON.Parse(JsonUtility.ToJson(this));

        node.Add("prestigePoints", prestigePoints.ToString());

        return node;
    }
}