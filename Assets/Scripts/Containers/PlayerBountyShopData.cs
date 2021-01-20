using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;

namespace GreedyMercs.BountyShop.Data
{
    public class PlayerBountyItem
    {
        public int totalBought;
    }

    public class PlayerBountyShopData
    {
        Dictionary<BountyShopItemID, PlayerBountyItem> items;

        public DateTime lastPurchaseReset;

        public PlayerBountyShopData(JSONNode node)
        {
            Update(node);
        }

        public void Update(JSONNode node)
        {
            if (node.HasKey("userBountyShop"))
                node = node["userBountyShop"];

            items = new Dictionary<BountyShopItemID, PlayerBountyItem>();

            foreach (string key in node["itemsBought"].Keys)
            {
                BountyShopItemID itemId = (BountyShopItemID)int.Parse(key);

                items[itemId] = new PlayerBountyItem { totalBought = node["itemsBought"][key].AsInt };
            }

            lastPurchaseReset = DateTimeOffset.FromUnixTimeMilliseconds(node["lastPurchaseReset"].AsLong).DateTime;
        }

        public PlayerBountyItem GetItem(BountyShopItemID key) => items[key];
    }
}
