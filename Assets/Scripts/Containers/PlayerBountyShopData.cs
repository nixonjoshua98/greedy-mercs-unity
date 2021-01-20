using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;

namespace GreedyMercs.Data.BountyShop
{
    public struct PlayerBountyShopItem
    {
        public int totalBought;
    }

    public class PlayerBountyShopData
    {
        Dictionary<BountyShopItemID, PlayerBountyShopItem> items;

        public PlayerBountyShopData(JSONNode node)
        {
            Update(node);
        }

        public void Update(JSONNode node)
        {
            if (node.HasKey("userBountyShop"))
                node = node["userBountyShop"];

            items = new Dictionary<BountyShopItemID, PlayerBountyShopItem>();

            foreach (string key in node["itemsBought"].Keys)
            {
                BountyShopItemID itemId = (BountyShopItemID)int.Parse(key);

                items[itemId] = new PlayerBountyShopItem { totalBought = node["itemsBought"][key].AsInt };
            }
        }

        public PlayerBountyShopItem GetItem(BountyShopItemID key) => items[key];
    }
}
