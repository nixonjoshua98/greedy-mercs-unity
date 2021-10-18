using GM.HTTP.Requests;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace GM.BountyShop.Data
{
    public class BountyShopDataCollection : Core.GMClass
    {
        Dictionary<string, BountyShopPurchaseData> Purchases;

        public List<BountyShopArmouryItem> ArmouryItems;

        public BountyShopDataCollection(Models.CompleteBountyShopDataModel data)
        {
            Purchases = new Dictionary<string, BountyShopPurchaseData>();

            ArmouryItems = data.ShopItems.ArmouryItems;
        }

        public IBountyShopItem GetItem(string id)
        {
            return GetArmouryItem(id);
        }


        public BountyShopArmouryItem GetArmouryItem(string id) => ArmouryItems.Where(ele => ele.Id == id).FirstOrDefault();
        public BountyShopCurrencyItemData GetCurrencyItem(string id) => null;
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


        public void PurchaseItem(string itemId, UnityAction<bool> action)
        {
            var req = new PurchaseBountyShopItemRequest { ShopItem = itemId };

            App.HTTP.BShop_PurchaseItem(req, (resp) =>
            {
                if (resp.StatusCode == 200)
                {
                    if (resp.ArmouryItem != null)
                        App.Data.Armoury.UpdateUserItem(resp.ArmouryItem);

                    App.Data.Inv.UpdateCurrencies(resp.CurrencyItems);
                }
            });
        }
    }
}