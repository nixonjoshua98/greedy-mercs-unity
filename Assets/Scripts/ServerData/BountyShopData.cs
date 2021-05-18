using SimpleJSON;

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GreedyMercs.BountyShop.Data
{
    public enum BountyShopItemID
    {
        NONE = -1,

        PRESTIGE_POINTS = 100,
        GEMS            = 101,

        ARMOURY_CHEST = 300,
    }

    public class BountyShopData 
    {
        Dictionary<BountyShopItemID, BountyShopItemData> items;

        public BountyShopData(JSONNode node)
        {
            items = new Dictionary<BountyShopItemID, BountyShopItemData>();

            foreach (string key in node.Keys)
            {
                BountyShopItemID itemId = (BountyShopItemID)int.Parse(key);
                BountyShopItemData item = new BountyShopItemData(node[key]);

                items.Add(itemId, item);
            }
        }

        public BountyShopItemData Get(BountyShopItemID key) => items[key];
    }
}