using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;

namespace GreedyMercs.BountyShop.Data
{
    public class BountyItemState
    {
        public int totalBought;
    }

    public class PlayerBountyShopData
    {
        Dictionary<BountyShopItemID, BountyItemState> items;

        public DateTime lastPurchaseReset;

        public PlayerBountyShopData(JSONNode node)
        {
            Update(node);
        }

        public void Update(JSONNode node)
        {
            items = new Dictionary<BountyShopItemID, BountyItemState>();

            foreach (string key in node["itemsBought"].Keys)
            {
                BountyShopItemID itemId = (BountyShopItemID)int.Parse(key);

                items[itemId] = new BountyItemState { totalBought = node["itemsBought"][key].AsInt };
            }

            lastPurchaseReset = node.HasKey("lastPurchaseReset") ? DateTimeOffset.FromUnixTimeMilliseconds(node["lastPurchaseReset"].AsLong).DateTime : lastPurchaseReset;
        }

        public JSONNode ToJson()
        {
            JSONNode node           = new JSONObject();
            JSONNode itemsBought    = new JSONObject();

            node.Add("lastPurchaseReset", lastPurchaseReset.ToUnixMilliseconds());

            foreach (var entry in items)
            { 
                itemsBought[((int)entry.Key).ToString()] = entry.Value.totalBought.ToString();
            }

            node.Add("itemsBought", itemsBought);

            return node;
        }


        public BountyItemState GetItem(BountyShopItemID key)
        {
            if (items.TryGetValue(key, out BountyItemState item))
                return item;

            items[key] = new BountyItemState { totalBought = 0 };

            return GetItem(key);
        }
    }
}
