﻿using GM.BountyShop.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace GM.BountyShop.Data
{
    public class BountyShopDataContainer : Core.GMClass
    {
        private Dictionary<string, int> itemPurchases = new Dictionary<string, int>();

        private Dictionary<string, BountyShopArmouryItem> armouryItems;
        private Dictionary<string, BountyShopCurrencyItem> currencyItems;

        DateTime ShopCreatedAt;

        // = Events = //
        public UnityEvent E_ShopUpdated = new();

        public bool IsValid => armouryItems is not null && currencyItems is not null && App.DailyRefresh.Current.IsBetween(ShopCreatedAt);

        public int GetItemPurchaseData(string id) => itemPurchases.Get(id, 0);

        private void UpdateItemPurchases(List<BountyShopPurchaseModel> purchaseList)
        {
            itemPurchases = new();

            foreach (var group in purchaseList.GroupBy(x => x.ItemID))
            {
                itemPurchases[group.Key] = group.Count();
            }
        }

        public List<BountyShopArmouryItem> ArmouryItems => armouryItems.Values.ToList();
        public List<BountyShopCurrencyItem> CurrencyItems => currencyItems.Values.ToList();

        /// <summary>
        /// Update all items
        /// </summary>
        private void UpdateShopItems(BountyShopItems items)
        {
            armouryItems = items.ArmouryItems.ToDictionary(ele => ele.ID, ele => ele);
            currencyItems = items.CurrencyItems.ToDictionary(ele => ele.ID, ele => ele);
        }

        public void FetchShop(UnityAction action)
        {
            App.HTTP.FetchBountyShop(resp =>
            {
                if (resp.StatusCode == HTTP.HTTPCodes.Success)
                {
                    ShopCreatedAt = resp.ShopCreationTime;

                    UpdateItemPurchases(resp.Purchases);
                    UpdateShopItems(resp.ShopItems);

                    E_ShopUpdated.Invoke();
                }

                action.Invoke();
            });
        }

        /// <summary>
        /// Send the server request for purchasing an armoury item
        /// </summary>
        public void PurchaseArmouryItem(string itemId, UnityAction<bool, PurchaseArmouryItemResponse> action)
        {
            App.HTTP.PurchaseBountyShopArmouryItem(itemId, (resp) =>
            {
                if (resp.StatusCode == 200)
                {
                    // Update purchase count
                    itemPurchases[itemId] = itemPurchases.Get(itemId, 0) + 1;

                    App.Armoury.Update(resp.ArmouryItem);

                    App.Inventory.UpdateCurrencies(resp.Currencies);
                }

                action.Invoke(resp.StatusCode == 200, resp);
            });
        }

        public void PurchaseCurrencyItem(string itemId, UnityAction<bool, PurchaseCurrencyResponse> action)
        {
            App.HTTP.PurchaseBountyShopCurrency(itemId, (resp) =>
            {
                if (resp.StatusCode == 200)
                {
                    // Update purchase count
                    itemPurchases[itemId] = itemPurchases.Get(itemId, 0) + 1;

                    App.Inventory.UpdateCurrencies(resp.Currencies);
                }

                action.Invoke(resp.StatusCode == 200, resp);
            });
        }
    }
}