using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


namespace GM.BountyShop
{
    using SimpleJSON;

    public enum BountyShopItemType
    {
        // Represents the type of item in the shop
        // The shop could contain multiple items (with unique ids) but multiple of the same item type

        FLAT_BP = 0, // Bounty Point [Temp]
        FLAT_BLUE_GEM = 100,
        FLAT_AP = 200, // Armoury Point

    }

    public class BountyShopItem
    {
        public int ID;

        public BountyShopItemType ItemType;

        public int QuantityPerPurchase;
        public int PurchaseCost;

        public BountyShopItem(int id, JSONNode node)
        {
            ID = id;

            ItemType = (BountyShopItemType)node["itemTypeId"].AsInt; // Quick conversion

            PurchaseCost = node["purchaseCost"].AsInt;

            QuantityPerPurchase = node["quantityPerPurchase"].AsInt;
        }
    }


    public class ServerBountyShopData
    {
        Dictionary<int, BountyShopItem> items;

        public ServerBountyShopData(JSONNode node)
        {
            UpdateAll(node);
        }

        // = = = GET = = =
        public List<BountyShopItem> Items { get { return items.Values.ToList(); } }
        public BountyShopItem GetItem(int id)
        {
            return items[id];
        }

        public void UpdateAll(JSONNode node)
        {
            UpdateItems(node["items"]);
        }

        public void UpdateItems(JSONNode node)
        {
            items = new Dictionary<int, BountyShopItem>();

            foreach (string key in node.Keys)
            {
                int id = int.Parse(key);

                items.Add(id, new BountyShopItem(id, node[key]));
            }
        }
    }
}