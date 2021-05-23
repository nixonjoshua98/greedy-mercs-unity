using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace GM.BountyShop
{
    using SimpleJSON;

    public enum BsTokenType
    {
        FLAT_BLUE_GEM   = 100,
        FLAT_AP         = 200, // Armoury Point
    }

    public abstract class AbstractBountyShopData
    {
        public int ID;

        public int DailyPurchaseLimit;
        public int PurchaseCost;

        string _IconString;

        public AbstractBountyShopData(int id, JSONNode node)
        {
            ID = id;

            _IconString = node["iconString"];

            PurchaseCost        = node["purchaseCost"].AsInt;
            DailyPurchaseLimit  = node["dailyPurchaseLimit"].AsInt;
        }

        public Sprite Icon { get { return ResourceManager.LoadSprite(_IconString); } }
    }


    public class BsItemData : AbstractBountyShopData
    {
        public BsTokenType Type;

        public int QuantityPerPurchase;

        public BsItemData(int id, JSONNode node) : base(id, node)
        {
            Type = (BsTokenType)node["itemType"].AsInt; // Quick conversion

            QuantityPerPurchase = node["quantityPerPurchase"].AsInt;
        }
    }


    public class ServerBountyShopData
    {
        Dictionary<int, BsItemData> itemData;

        public ServerBountyShopData()
        {
            itemData = new Dictionary<int, BsItemData>();
        }


        // = = = GET = = =
        public List<AbstractBountyShopData> All
        { 
            get
            {
                List<AbstractBountyShopData> ls = new List<AbstractBountyShopData>();

                foreach (AbstractBountyShopData ele in itemData.Values.ToList()) { ls.Add(ele); }

                return ls;
            } 
        }

        public AbstractBountyShopData Get(int id)
        {
            return itemData[id];
        }

        public void UpdateAll(JSONNode node)
        {
            SetItemsData(node["items"]);
        }


        public void SetItemsData(JSONNode node) { Set(ref itemData, node); }


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