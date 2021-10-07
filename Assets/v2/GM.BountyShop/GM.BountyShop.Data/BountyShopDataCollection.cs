using GM.HTTP.Requests;
using Newtonsoft.Json;
using SimpleJSON;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace GM.BountyShop.Data
{
    public class BountyShopDataCollection : Core.GMClass
    {
        Dictionary<string, BountyShopPurchaseData> Purchases;

        public List<BountyShopCurrencyItemData> CurrencyItems;
        public List<BountyShopArmouryItem> ArmouryItems;

        public BountyShopDataCollection(JSONNode json)
        {
            SetDailyPurchases(json["dailyPurchases"]);
            SetAvailableItems(json["availableItems"]);
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


        public void SetAvailableItems(JSONNode node)
        {
            CurrencyItems = JsonConvert.DeserializeObject<List<BountyShopCurrencyItemData>>(node["items"].ToString());
            ArmouryItems = JsonConvert.DeserializeObject<List<BountyShopArmouryItem>>(node["armouryItems"].ToString());
        }


        public void SetDailyPurchases(JSONNode node)
        {
            Purchases = new Dictionary<string, BountyShopPurchaseData>();

            foreach (string key in node.Keys)
            {
                var inst = new BountyShopPurchaseData
                {
                    TotalDailyPurchases = node[key].AsInt
                };

                Purchases.Add(key, inst);
            }
        }

        public void SetDailyPurchases(Dictionary<string, int> data)
        {
            Purchases = new Dictionary<string, BountyShopPurchaseData>();

            foreach (var pair in data)
            {
                var inst = new BountyShopPurchaseData
                {
                    TotalDailyPurchases = pair.Value
                };

                Purchases.Add(pair.Key, inst);
            }
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