using GM.HTTP.Requests;
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

        public List<BountyShopArmouryItem> ArmouryItems => ArmouryItemsDict.Values.ToList();
        void Update(List<BountyShopArmouryItem> items) => ArmouryItemsDict = items.ToDictionary(ele => ele.Id, ele => ele);
        public void PurchaseItem(string itemId, UnityAction<bool> action)
        {
            var req = new PurchaseBountyShopItemRequest { ShopItem = itemId };

            App.HTTP.BShop_PurchaseItem(req, (resp) =>
            {
                if (resp.StatusCode == 200)
                {
                    if (resp.ArmouryItem != null)
                        App.Data.Armoury.Update(resp.ArmouryItem);

                    App.Data.Inv.UpdateCurrencies(resp.CurrencyItems);
                }

                action.Invoke(resp.StatusCode == 200);
            });
        }
    }
}