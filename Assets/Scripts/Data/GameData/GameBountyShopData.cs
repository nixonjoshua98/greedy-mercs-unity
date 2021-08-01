using SimpleJSON;
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Data
{
    public abstract class AbstractBountyShopItem
    {
        public string ID;

        public int DailyPurchaseLimit;
        public int PurchaseCost;

        public AbstractBountyShopItem(string itemId, JSONNode node)
        {
            ID = itemId;

            PurchaseCost        = node["purchaseCost"].AsInt;
            DailyPurchaseLimit  = node["dailyPurchaseLimit"].AsInt;
        }

        public abstract Sprite Icon { get; }
    }


    public class BountyShopItem : AbstractBountyShopItem
    {
        public readonly ItemType ItemID;

        public int QuantityPerPurchase;

        public BountyShopItem(string itemId, JSONNode node) : base(itemId, node)
        {
            ItemID = (ItemType)Enum.Parse(typeof(ItemType), node["itemType"]);

            QuantityPerPurchase = node["quantityPerPurchase"].AsInt;
        }

        public override Sprite Icon => GameData.Get.Items.Get(ItemID).Icon;
    }


    public class BountyShopArmouryItem : AbstractBountyShopItem
    {
        public int ArmouryItemID;

        public BountyShopArmouryItem(string itemId, JSONNode node) : base(itemId, node)
        {
            ArmouryItemID = node["armouryItemId"].AsInt;
        }

        public override Sprite Icon { get { return GameData.Get.Armoury.Get(ArmouryItemID).Icon; } }
    }




    public class GameBountyShopData
    {
        public Dictionary<string, BountyShopItem> items;
        public Dictionary<string, BountyShopArmouryItem> armouryItems;

        public AbstractBountyShopItem Get(string itemId)
        {
            if (items.ContainsKey(itemId))
                return items[itemId];

            return armouryItems[itemId];
        }


        public BountyShopItem GetItem(string id) => items[id];
        public BountyShopArmouryItem GetArmouryItem(string id) => armouryItems[id];


        public List<BountyShopItem> Items => items.Values.ToList();
        public List<BountyShopArmouryItem> ArmouryItems => armouryItems.Values.ToList();


        public void UpdateAll(JSONNode node)
        {
            CreateItemDictionary(ref items, node["items"]);
            CreateItemDictionary(ref armouryItems, node["armouryItems"]);

            Debug.Log(string.Join(", ", items.Keys));
            Debug.Log(string.Join(", ", armouryItems.Keys));
        }



        void CreateItemDictionary<TVal>(ref Dictionary<string, TVal> dict, JSONNode node) where TVal : AbstractBountyShopItem
        {
            dict = new Dictionary<string, TVal>();

            foreach (string key in node.Keys)
            {
                TVal instance = (TVal)Activator.CreateInstance(typeof(TVal), new object[] { key, node[key] });

                dict.Add(key, instance);
            }
        }
    }
}
