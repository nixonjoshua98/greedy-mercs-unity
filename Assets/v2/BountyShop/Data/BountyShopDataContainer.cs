using GM.BountyShop.Models;
using GM.Common.Enums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace GM.BountyShop.Data
{
    public class BountyShopDataContainer : Core.GMClass
    {
        Dictionary<string, int> itemPurchases = new Dictionary<string, int>();

        Dictionary<string, BountyShopArmouryItem> armouryItems = new Dictionary<string, BountyShopArmouryItem>();
        Dictionary<string, BountyShopCurrencyItemModel> currencyItems = new Dictionary<string, BountyShopCurrencyItemModel>();

        public void Set(CompleteBountyShopDataModel data)
        {
            Update(data.ShopItems);
            UpdateItemPurchases(data.Purchases);
        }

        public int GetItemPurchaseData(string id) => itemPurchases.Get(id, 0);

        void UpdateItemPurchases(List<BountyShopPurchaseModel> purchaseList)
        {
            itemPurchases.Clear();

            foreach (var group in purchaseList.GroupBy(x => x.ItemId))
            {
                itemPurchases[group.Key] = group.Count();
            }
        }

        public List<BountyShopArmouryItem> ArmouryItems => armouryItems.Values.ToList();
        public List<BountyShopCurrencyItemModel> CurrencyItems => currencyItems.Values.ToList();

        /// <summary>
        /// Update all items
        /// </summary>
        void Update(BountyShopItemsModel items)
        {
            armouryItems = items.ArmouryItems.ToDictionary(ele => ele.ID, ele => ele);
            currencyItems = items.CurrencyItems.ToDictionary(ele => ele.ID, ele => ele);
        }

        /// <summary>
        /// Update all bounty shop related data
        /// </summary>
        public void UpdateShop(CompleteBountyShopDataModel model)
        {
            UpdateItemPurchases(model.Purchases);
            Update(model.ShopItems);
        }

        /// <summary>Send the server request for purchasing an armoury item</summary>
        public void PurchaseArmouryItem(string itemId, UnityAction<bool> action)
        {
            //App.HTTP.BuyBountyShopArmouryItem(itemId, (resp) =>
            //{
            //    if (resp.StatusCode == 200)
            //    {
            //        App.Armoury.Update(resp.ArmouryItem);

            //        OnAnySuccessfullPurchase(itemId, resp);
            //    }

            //    action.Invoke(resp.StatusCode == 200);
            //});
        }

        public void PurchaseCurrencyItem(string itemId, UnityAction<bool> action)
        {
            App.HTTP.PurchaseBountyShopCurrency(itemId, (resp) =>
            {
                if (resp.StatusCode == 200)
                {
                    // Update purchase count
                    itemPurchases[itemId] = itemPurchases.Get(itemId, 0) + 1;

                    App.Inventory.UpdateCurrencies(resp.Currencies);
                }

                action.Invoke(resp.StatusCode == 200);
            });
        }
    }
}