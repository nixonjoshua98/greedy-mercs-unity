﻿using GM.HTTP.Requests;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace GM.BountyShop.Data
{
    public class BountyShopDataCollection : Core.GMClass
    {
        Dictionary<string, BountyShopPurchaseData> Purchases;
        Dictionary<string, BountyShopArmouryItem> ArmouryItemsDict;

        public BountyShopDataCollection(Models.CompleteBountyShopDataModel data)
        {
            Purchases = new Dictionary<string, BountyShopPurchaseData>();

            Update(data.ShopItems.ArmouryItems);
        }

        public BountyShopPurchaseData GetItemPurchaseData(string id)
        {
            if (!Purchases.ContainsKey(id))
            {
                return new BountyShopPurchaseData
                {
                    TotalDailyPurchases = 0
                };
            }

            return Purchases[id];
        }

        /// <summary>
        /// Public accessor for only armoury items
        /// </summary>
        public List<BountyShopArmouryItem> ArmouryItems => ArmouryItemsDict.Values.ToList();

        /// <summary>
        /// Update all items
        /// </summary>
        void Update(List<BountyShopArmouryItem> items) => ArmouryItemsDict = items.ToDictionary(ele => ele.Id, ele => ele);

        /// <summary>
        /// Send the server request for purchasing an armoury item
        /// </summary>
        public void PurchaseArmouryItem(string itemId, UnityAction<bool> action)
        {
            var req = new PurchaseBountyShopItemRequest { ShopItem = itemId };

            App.HTTP.BShop_PurchaseArmouryItem(req, (resp) =>
            {
                if (resp.StatusCode == 200)
                {
                    App.Data.Armoury.Update(resp.ArmouryItem);

                    App.Data.Inv.UpdateCurrencies(resp.CurrencyItems);
                }

                action.Invoke(resp.StatusCode == 200);
            });
        }
    }
}