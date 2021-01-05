﻿using System.Numerics;

using SimpleJSON;

using UnityEngine;


[System.Serializable]
public class PlayerState
{
    public BigDouble gold = 0;
    public BigInteger prestigePoints = 0;

    public int bountyPoints = 0;

    public void OnRestore(JSONNode node)
    {
        if (node.HasKey("player"))
        {
            JSONNode player = node["player"];

            gold = player.HasKey("gold") ? BigDouble.Parse(player["gold"].Value) : gold;

            bountyPoints    = player.HasKey("bountyPoints") ? int.Parse(node["bountyPoints"].Value) : bountyPoints;
            prestigePoints  = player.HasKey("prestigePoints") ? BigInteger.Parse(player["prestigePoints"].Value, System.Globalization.NumberStyles.Any) : prestigePoints;
        }
    }

    public void Update(JSONNode node)
    {
        bountyPoints    = node.HasKey("bountyPoints") ? node["bountyPoints"].AsInt : bountyPoints;
        prestigePoints  = node.HasKey("prestigePoints") ? BigInteger.Parse(node["prestigePoints"].Value, System.Globalization.NumberStyles.Any) : prestigePoints;
    }

    public JSONNode ToJson()
    {
        JSONNode node = new JSONObject();

        node.Add("gold", gold.ToString().Replace("E", "e"));
        node.Add("prestigePoints", prestigePoints.ToString());

        return node;
    }
}