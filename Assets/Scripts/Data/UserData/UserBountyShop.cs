using System;
using System.Linq;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace GM.Bounties
{
    using GM.Data;

    using GM.HTTP;

    using SimpleJSON;

    #region Dataclasses
    struct BountyShopPurchaseData
    {
        public int TotalDailyPurchases;

        public BountyShopPurchaseData(int total)
        {
            TotalDailyPurchases = total;
        }
    }

    public abstract class AbstractBountyShopItem
    {
        public string ID;

        public int DailyPurchaseLimit;
        public int PurchaseCost;

        public AbstractBountyShopItem(string itemId, JSONNode node)
        {
            ID = itemId;

            PurchaseCost = node["purchaseCost"].AsInt;
            DailyPurchaseLimit = node["dailyPurchaseLimit"].AsInt;
        }

        public abstract Sprite Icon { get; }
    }


    public class BountyShopItem : AbstractBountyShopItem
    {
        public readonly GM.Items.Data.ItemType ItemID;

        public int QuantityPerPurchase;

        public BountyShopItem(string itemId, JSONNode node) : base(itemId, node)
        {
            ItemID = (GM.Items.Data.ItemType)Enum.Parse(typeof(GM.Items.Data.ItemType), node["itemType"]);

            QuantityPerPurchase = node["quantityPerPurchase"].AsInt;
        }

        public Items.Data.FullGameItemData ItemData => Core.GMApplication.Instance.Data.Items[ItemID];

        public override Sprite Icon => ItemData.Icon;
    }


    public class BountyShopArmouryItem : AbstractBountyShopItem
    {
        public int ArmouryItemID;

        public BountyShopArmouryItem(string itemId, JSONNode node) : base(itemId, node)
        {
            ArmouryItemID = node["armouryItemId"].AsInt;
        }

        public GM.Armoury.Data.ArmouryItemGameData ArmouryItem => Core.GMApplication.Instance.Data.Armoury.Game[ArmouryItemID];
        public override Sprite Icon => ArmouryItem.Icon;
    }
    #endregion

    public class UserBountyShop : Core.GMClass
    {
        Dictionary<string, BountyShopPurchaseData> purchases;

        Dictionary<string, BountyShopItem> availItems;
        Dictionary<string, BountyShopArmouryItem> availArmouryItems;

        public BountyShopItem[] Items => availItems.Values.ToArray();
        public BountyShopArmouryItem[] ArmouryItems => availArmouryItems.Values.ToArray();


        public UserBountyShop(JSONNode node)
        {
            SetDailyPurchases(node["dailyPurchases"]);
            SetAvailableItems(node["availableItems"]);
        }


        public AbstractBountyShopItem Get(string itemId)
        {
            if (availItems.ContainsKey(itemId))
                return availItems[itemId];

            return availArmouryItems[itemId];
        }


        public BountyShopItem GetItem(string id) => availItems[id];
        public BountyShopArmouryItem GetArmouryItem(string id) => availArmouryItems[id];


        public void SetAvailableItems(JSONNode node)
        {
            CreateItemDictionary(ref availItems, node["items"]);
            CreateItemDictionary(ref availArmouryItems, node["armouryItems"]);
        }


        public void SetDailyPurchases(JSONNode node)
        {
            purchases = new Dictionary<string, BountyShopPurchaseData>();

            // {0: 0, 1: 3}
            foreach (string key in node.Keys)
            {
                BountyShopPurchaseData inst = new BountyShopPurchaseData(node[key].AsInt);

                purchases.Add(key, inst);
            }
        }


        // = = = Quick Helper = = = //
        public bool InStock(string itemId)
        {
            int dailyPurchases = 0;

            if (purchases.TryGetValue(itemId, out BountyShopPurchaseData data))
                dailyPurchases = data.TotalDailyPurchases;

            return Get(itemId).DailyPurchaseLimit > dailyPurchases;
        }


        // = = = Server Methods ===
        public void Refresh(UnityAction action)
        {
            void InternalCallback(long code, JSONNode resp)
            {
                if (code == 200)
                {
                    // Update the store items pulled from the server
                    SetAvailableItems(resp["bountyShopItems"]);

                    OnServerResponse(resp);

                    action.Invoke();
                }
            }

            HTTPClient.Instance.Post("bountyshop/refresh", InternalCallback);
        }


        public void PurchaseItem(string itemId, UnityAction<bool> action)
        {
            void Callback(long code, JSONNode resp)
            {
                if (code == 200)
                {
                    OnServerResponse(resp);
                }

                action.Invoke(code == 200);
            }

            HTTPClient.Instance.Post("bountyshop/purchase/item", CreateJson(itemId), Callback);
        }


        public void PurchaseArmouryItem(string itemId, UnityAction<bool> action)
        {
            void Callback(long code, JSONNode resp)
            {
                if (code == 200)
                {
                   App.Data.Armoury.User.UpdateWithJSON(resp["userArmouryItems"]);

                    OnServerResponse(resp);
                }

                action.Invoke(code == 200);
            }

            HTTPClient.Instance.Post("bountyshop/purchase/armouryitem", CreateJson(itemId), Callback);
        }


        // = = = Helper = = = //
        JSONNode CreateJson(string itemId)
        {
            JSONNode node = new JSONObject();

            node["shopItem"] = itemId;

            return node;
        }


        // = = = Callbacks = = = //
        void OnServerResponse(JSONNode resp)
        {
            App.Data.Inv.UpdateWithJSON(resp["userItems"]);

            SetDailyPurchases(resp["dailyPurchases"]);
        }


        // = = =
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