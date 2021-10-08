using GM.HTTP.Requests;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace GM.BountyShop.Data
{
    public class BountyShopDataCollection : Core.GMClass
    {
        Dictionary<string, BountyShopPurchaseData> Purchases;

        public List<BountyShopCurrencyItemData> CurrencyItems;
        public List<BountyShopArmouryItem> ArmouryItems;

        public BountyShopDataCollection(Models.CompleteBountyShopDataModel data)
        {
            Purchases = new Dictionary<string, BountyShopPurchaseData>();

            CurrencyItems = data.ShopItems.CurrencyItems;
            ArmouryItems = data.ShopItems.ArmouryItems;
        }

        public IBountyShopItem GetItem(string id)
        {
            BountyShopCurrencyItemData item = GetCurrencyItem(id);

            if (item != null)
                return item;

            return GetArmouryItem(id);
        }


        public BountyShopCurrencyItemData GetCurrencyItem(string id) => CurrencyItems.Where(ele => ele.Id == id).FirstOrDefault();
        public BountyShopArmouryItem GetArmouryItem(string id) => ArmouryItems.Where(ele => ele.Id == id).FirstOrDefault();

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
                        App.Data.Armoury.User.Update(resp.ArmouryItem);

                    App.Data.Inv.UpdateCurrencies(resp.CurrencyItems);
                }
            });
        }
    }
}