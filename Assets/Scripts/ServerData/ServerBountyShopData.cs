using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace GM.BountyShop
{
    using SimpleJSON;

    public enum BsItemType
    {
        BLUE_GEM        = 100,
        ARMOURY_POINTS  = 200,
        PRESTIGE_POINTS = 300,
    }

    public abstract class AbstractBountyShopData
    {
        public int ID;

        public int QuantityPerPurchase;
        public int DailyPurchaseLimit;
        public int PurchaseCost;

        public AbstractBountyShopData(int id, JSONNode node)
        {
            ID = id;

            PurchaseCost        = node["purchaseCost"].AsInt;
            DailyPurchaseLimit  = node["dailyPurchaseLimit"].AsInt;
            QuantityPerPurchase = node["quantityPerPurchase"].AsInt;
        }

        public abstract Sprite Icon { get; }
    }


    public class BsItemData : AbstractBountyShopData
    {
        public BsItemType ItemType;

        string _IconString;

        public BsItemData(int id, JSONNode node) : base(id, node)
        {
            ItemType = (BsItemType)node["itemType"].AsInt;

            _IconString = node["iconString"];
        }

        public override Sprite Icon { get { return ResourceManager.LoadSprite(_IconString); } }
    }

    public class BsArmouryItemData : AbstractBountyShopData
    {
        public int ArmouryItemID;

        public BsArmouryItemData(int id, JSONNode node) : base(id, node)
        {
            ArmouryItemID = node["armouryItemId"].AsInt;
        }

        public override Sprite Icon { get { return StaticData.Armoury.Get(ArmouryItemID).Icon; } }
    }



    public class ServerBountyShopData
    {
        Dictionary<int, BsItemData> normalItems;
        Dictionary<int, BsArmouryItemData> armouryItems;

        // = = = GET = = =
        public List<BsItemData> NormalItems { get { return normalItems.Values.ToList(); } }
        public List<BsArmouryItemData> ArmouryItems { get { return armouryItems.Values.ToList(); } }

        public AbstractBountyShopData Get(int id)
        {
            if (normalItems.ContainsKey(id))
                return normalItems[id];

            return armouryItems[id];
        }

        public BsArmouryItemData GetArmouryItem(int id)
        {
            return armouryItems[id];
        }

        public void UpdateAll(JSONNode node)
        {
            Set(ref normalItems, node["items"]);
            Set(ref armouryItems, node["armouryItems"]);
        }


        // = = = Private Methods = = =
        void Set<TVal>(ref Dictionary<int, TVal> dict, JSONNode node)
        {
            dict = new Dictionary<int, TVal>();

            foreach (string key in node.Keys)
            {
                int id = int.Parse(key);

                // Smelly code to create an instance of the shop item from a generic
                TVal instance = (TVal)Activator.CreateInstance(typeof(TVal), new object[] { id, node[key] });

                dict.Add(id, instance);
            }
        }
    }
}