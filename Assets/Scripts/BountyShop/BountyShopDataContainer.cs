using SRC.BountyShop.Requests;
using SRC.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using SRC.Common;

namespace SRC.BountyShop.Data
{
    public class BountyShopDataContainer : Core.GMClass
    {
        public int GameDayNumber;

        List<BountyShopPurchaseModel> ItemPurchases = new();

        private Dictionary<string, BountyShopArmouryItem> ArmouryItemsLookup;
        private Dictionary<string, BountyShopCurrencyItem> CurrencyItemsLookup;

        public bool IsValid => Utility.GetGameDayNumber() == GameDayNumber;

        public BountyShopPurchaseModel GetItemPurchase(BountyShopItemType itemType, string itemId)
        {
            return ItemPurchases.FirstOrDefault(p => p.ItemType == itemType && p.ItemID == itemId);
        }

        public TimeSpan TimeUntilRefresh => Utility.GetGameDayDate(GameDayNumber + 1) - DateTime.UtcNow;

        public List<BountyShopArmouryItem> ArmouryItems => ArmouryItemsLookup.Values.ToList();
        public List<BountyShopCurrencyItem> CurrencyItems => CurrencyItemsLookup.Values.ToList();

        public void FetchShop(UnityAction action)
        {
            App.HTTP.FetchBountyShop(resp =>
            {
                if (resp.StatusCode == HTTP.HTTPCodes.Success)
                {
                    ItemPurchases = resp.Purchases;
                    GameDayNumber = resp.GameDayNumber;

                    ArmouryItemsLookup    = resp.ShopItems.ArmouryItems.ToDictionary(ele => ele.ID, ele => ele);
                    CurrencyItemsLookup   = resp.ShopItems.CurrencyItems.ToDictionary(ele => ele.ID, ele => ele);
                }

                action.Invoke();
            });
        }

        public void PurchaseArmouryItem(string itemId, UnityAction<PurchaseArmouryItemResponse> action)
        {
            App.HTTP.PurchaseBountyShopArmouryItem(itemId, resp =>
            {
                if (resp.IsSuccess)
                {
                    App.Armoury.Update(resp.ArmouryItem);
                    App.Inventory.UpdateCurrencies(resp.Currencies);
                    ItemPurchases.Add(new(itemId, BountyShopItemType.ArmouryItem));
                }

                action.Invoke(resp);
            });
        }

        public void PurchaseCurrencyItem(string itemId, UnityAction<PurchaseCurrencyResponse> action)
        {
            App.HTTP.PurchaseBountyShopCurrency(itemId, (resp) =>
            {
                if (resp.StatusCode == 200)
                {
                    App.Inventory.UpdateCurrencies(resp.Currencies);
                    ItemPurchases.Add(new(itemId, BountyShopItemType.CurrencyItem));
                }

                action.Invoke(resp);
            });
        }
    }
}