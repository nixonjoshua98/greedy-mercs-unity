using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using GM.BountyShop.Models;
using UnityEngine;

namespace GM.BountyShop.Data
{
    public class BountyShopData : Core.GMClass
    {
        Dictionary<string, int> itemPurchases = new Dictionary<string, int>();

        Dictionary<string, BountyShopArmouryItem> ArmouryItemsDict;

        public BountyShopData(CompleteBountyShopDataModel data)
        {
            Update(data.ShopItems.ArmouryItems);
            GetNumItemPurchases(data.Purchases);
        }

        public int GetItemPurchaseData(string id)
        {
            return itemPurchases.Get(id, 0);
        }

        void GetNumItemPurchases(List<BountyShopPurchaseModel> purchaseList)
        {
            itemPurchases.Clear();

            foreach (var group in purchaseList.GroupBy(x => x.ItemId))
            {
                itemPurchases[group.Key] = group.Count();
            }
        }

        /// <summary>Public accessor for only armoury items</summary>
        public List<BountyShopArmouryItem> ArmouryItems => ArmouryItemsDict.Values.ToList();

        /// <summary>Update all items</summary>
        void Update(List<BountyShopArmouryItem> items) => ArmouryItemsDict = items.ToDictionary(ele => ele.Id, ele => ele);

        /// <summary>Send the server request for purchasing an armoury item</summary>
        public void PurchaseArmouryItem(string itemId, UnityAction<bool> action)
        {
            App.HTTP.BuyBountyShopArmouryItem(itemId, (resp) =>
            {
                if (resp.StatusCode == 200)
                {
                    itemPurchases[itemId] = itemPurchases.Get(itemId, 0) + 1; // Increment purchase count

                    App.Data.Armoury.Update(resp.ArmouryItem);

                    App.Data.Inv.UpdateCurrencies(resp.CurrencyItems);

                    App.Events.BountyPointsChanged.Invoke(resp.PurchaseCost * -1);
                }

                action.Invoke(resp.StatusCode == 200);
            });
        }
    }
}