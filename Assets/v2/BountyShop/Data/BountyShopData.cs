using GM.BountyShop.Models;
using GM.Common.Enums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace GM.BountyShop.Data
{
    public class BountyShopData : Core.GMClass
    {
        Dictionary<string, int> itemPurchases = new Dictionary<string, int>();

        Dictionary<string, BountyShopArmouryItem> armouryItems = new Dictionary<string, BountyShopArmouryItem>();
        Dictionary<string, BountyShopCurrencyItemModel> currencyItems = new Dictionary<string, BountyShopCurrencyItemModel>();

        public BountyShopData(CompleteBountyShopDataModel data)
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

        /// <summary>Update all items</summary>
        void Update(BountyShopItemsModel items)
        {
            armouryItems = items.ArmouryItems.ToDictionary(ele => ele.Id, ele => ele);
            currencyItems = items.CurrencyItems.ToDictionary(ele => ele.Id, ele => ele);
        }

        /// <summary>Send the server request for purchasing an armoury item</summary>
        public void PurchaseArmouryItem(string itemId, UnityAction<bool> action)
        {
            App.HTTP.BuyBountyShopArmouryItem(itemId, (resp) =>
            {
                if (resp.StatusCode == 200)
                {
                    App.Data.Armoury.Update(resp.ArmouryItem);

                    OnAnySuccessfullPurchase(itemId, resp);
                }

                action.Invoke(resp.StatusCode == 200);
            });
        }

        public void PurchaseCurrencyItem(string itemId, UnityAction<bool> action)
        {
            App.HTTP.PurchaseBountyShopCurrencyType(itemId, (resp) =>
            {
                if (resp.StatusCode == 200)
                {
                    OnAnySuccessfullPurchase(itemId, resp);

                    switch (currencyItems[itemId].Item.Item)
                    {
                        case CurrencyType.ARMOURY_POINTS:
                            App.Events.ArmouryPointsChanged.Invoke(resp.CurrencyGained);
                            break;
                    };
                }

                action.Invoke(resp.StatusCode == 200);
            });
        }

        void OnAnySuccessfullPurchase(string itemId, GM.HTTP.Requests.BountyShop.BountyShopPurchaseResponse resp)
        {
            itemPurchases[itemId] = itemPurchases.Get(itemId, 0) + 1;

            App.Data.Inv.UpdateCurrencies(resp.CurrencyItems);

            App.Events.BountyPointsChanged.Invoke(resp.PurchaseCost * -1);
        }
    }
}