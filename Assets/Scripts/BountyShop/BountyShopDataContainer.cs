using SRC.BountyShop.Requests;
using SRC.Common.Enums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace SRC.BountyShop.Data
{
    public class BountyShopDataContainer : Core.GMClass
    {
        public int GameDayNumber;

        List<BountyShopPurchaseModel> ItemPurchases = new();

        private Dictionary<string, BountyShopArmouryItem> armouryItems;
        private Dictionary<string, BountyShopCurrencyItem> currencyItems;

        public bool IsValid => SRC.Common.Utility.GetGameDayNumber() == GameDayNumber;

        public BountyShopPurchaseModel GetItemPurchase(BountyShopItemType itemType, string itemId)
        {
            return ItemPurchases.FirstOrDefault(p => p.ItemType == itemType && p.ItemID == itemId);
        }

        public List<BountyShopArmouryItem> ArmouryItems => armouryItems.Values.ToList();
        public List<BountyShopCurrencyItem> CurrencyItems => currencyItems.Values.ToList();

        public void FetchShop(UnityAction action)
        {
            App.HTTP.FetchBountyShop(resp =>
            {
                if (resp.StatusCode == HTTP.HTTPCodes.Success)
                {
                    ItemPurchases = resp.Purchases;
                    GameDayNumber = resp.GameDayNumber;

                    armouryItems    = resp.ShopItems.ArmouryItems.ToDictionary(ele => ele.ID, ele => ele);
                    currencyItems   = resp.ShopItems.CurrencyItems.ToDictionary(ele => ele.ID, ele => ele);
                }

                action.Invoke();
            });
        }

        /// <summary>
        /// Send the server request for purchasing an armoury item
        /// </summary>
        public void PurchaseArmouryItem(string itemId, UnityAction<PurchaseArmouryItemResponse> action)
        {
            App.HTTP.PurchaseBountyShopArmouryItem(itemId, resp =>
            {
                if (resp.IsSuccess)
                {
                    App.Armoury.Update(resp.ArmouryItem);

                    App.Inventory.UpdateCurrencies(resp.Currencies);
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
                }

                action.Invoke(resp);
            });
        }
    }
}