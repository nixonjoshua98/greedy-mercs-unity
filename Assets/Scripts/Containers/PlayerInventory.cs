using System;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleJSON;


namespace GreedyMercs.Inventory
{
    public class PlayerInventory
    {
        public BigInteger prestigePoints = 0;

        public long gems            = 0;
        public long bountyPoints    = 0;
        public long armouryPoints   = 0;

        public PlayerInventory(JSONNode node)
        {
            Update(node);
        }

        public void Update(JSONNode node)
        {
            UpdateCurrencies(node);
        }

        void UpdateCurrencies(JSONNode node)
        {
            gems = node.HasKey("gems") ? node["gems"].AsLong : gems;

            bountyPoints    = node.HasKey("bountyPoints") ? node["bountyPoints"].AsLong : bountyPoints;
            armouryPoints   = node.HasKey("armouryPoints") ? node["armouryPoints"].AsLong : armouryPoints;

            prestigePoints = node.HasKey("prestigePoints") ? BigInteger.Parse(node["prestigePoints"].Value, System.Globalization.NumberStyles.Any) : prestigePoints;
        }

        public JSONNode ToJson()
        {
            JSONNode node = new JSONObject();

            node.Add("bountyPoints", bountyPoints.ToString());
            node.Add("armouryPoints", armouryPoints.ToString());
            node.Add("prestigePoints", prestigePoints.ToString());

            return node;
        }
    }
}