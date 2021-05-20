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

        FLAT_BLUE_GEM   = 100,
        FLAT_AP         = 200, // Armoury Point

    }

    public struct BountyShopItemData
    {
        public int ID;

        public BountyShopItemType ItemType;

        public int QuantityPerPurchase;
        public int DailyPurchaseLimit;
        public int PurchaseCost;

        public BountyShopItemData(int id, JSONNode node)
        {
            ID = id;

            ItemType = (BountyShopItemType)node["itemTypeId"].AsInt; // Quick conversion

            PurchaseCost = node["purchaseCost"].AsInt;

            QuantityPerPurchase = node["quantityPerPurchase"].AsInt;
            DailyPurchaseLimit = node["dailyPurchaseLimit"].AsInt;
        }
    }


    public class ServerBountyShopData
    {
        Dictionary<int, BountyShopItemData> items;

        public ServerBountyShopData(JSONNode node)
        {
            UpdateAll(node);
        }

        // = = = GET = = =
        public List<BountyShopItemData> Items { get { return items.Values.ToList(); } }
        public BountyShopItemData GetItem(int id)
        {
            return items[id];
        }

        public void UpdateAll(JSONNode node)
        {
            UpdateItems(node["items"]);
        }

        public void UpdateItems(JSONNode node)
        {
            items = new Dictionary<int, BountyShopItemData>();

            foreach (string key in node.Keys)
            {
                int id = int.Parse(key);

                items.Add(id, new BountyShopItemData(id, node[key]));
            }
        }
    }
}