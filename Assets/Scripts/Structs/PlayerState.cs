using System.Numerics;

using SimpleJSON;


[System.Serializable]
public class PlayerState
{
    public BigDouble gold = 0;

    public BigInteger bountyPoints      = 0;
    public BigInteger prestigePoints    = 0;

    public void OnRestore(JSONNode node)
    {
        Update(node);
    }

    public void Update(JSONNode node)
    {
        if (node.HasKey("player"))
            node = node["player"];

        gold = node.HasKey("gold") ? BigDouble.Parse(node["gold"].Value) : gold;

        bountyPoints    = node.HasKey("bountyPoints")   ? BigInteger.Parse(node["bountyPoints"].Value, System.Globalization.NumberStyles.Any)   : bountyPoints;
        prestigePoints  = node.HasKey("prestigePoints") ? BigInteger.Parse(node["prestigePoints"].Value, System.Globalization.NumberStyles.Any) : prestigePoints;
    }

    public JSONNode ToJson()
    {
        JSONNode node = new JSONObject();

        node.Add("gold", gold.ToString().Replace("E", "e"));
        node.Add("bountyPoints", bountyPoints.ToString());
        node.Add("prestigePoints", prestigePoints.ToString());

        return node;
    }
}