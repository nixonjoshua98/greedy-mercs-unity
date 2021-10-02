using GM.HTTP;
using Newtonsoft.Json;
using SimpleJSON;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace GM.BountyShop.Data
{
    public class BountyShopDataCollection : Core.GMClass
    {
        Dictionary<string, BountyShopPurchaseUserData> Purchases;

        public List<BountyShopCurrencyItemData> CurrencyItem;
        public List<BountyShopArmouryItemData> ArmouryItems;

        public BountyShopDataCollection(JSONNode node)
        {
            SetDailyPurchases(node["dailyPurchases"]);
            SetAvailableItems(node["availableItems"]);
        }


        public IBountyShopItem GetItem(string id)
        {
            BountyShopCurrencyItemData item = GetCurrencyItem(id);

            if (item != null)
                return item;

            return GetArmouryItem(id);
        }


        public BountyShopCurrencyItemData GetCurrencyItem(string id) => CurrencyItem.Where(ele => ele.Id == id).FirstOrDefault();
        public BountyShopArmouryItemData GetArmouryItem(string id) => ArmouryItems.Where(ele => ele.Id == id).FirstOrDefault();

        public BountyShopPurchaseUserData GetItemPurchaseData(string id)
        {
            if (!Purchases.ContainsKey(id))

            return Purchases[id];
        }


        public void SetAvailableItems(JSONNode node)
        {
            CurrencyItem = JsonConvert.DeserializeObject<List<BountyShopCurrencyItemData>>(node["items"].ToString());
            ArmouryItems = JsonConvert.DeserializeObject<List<BountyShopArmouryItemData>>(node["armouryItems"].ToString());
        }


        public void SetDailyPurchases(JSONNode node)
        {
            Purchases = new Dictionary<string, BountyShopPurchaseUserData>();

            // {0: 0, 1: 3}
            foreach (string key in node.Keys)
            {

                Purchases.Add(key, inst);
            }
        }


        public void PurchaseItem(string itemId, UnityAction<bool> action)
        {
            void Callback(long code, JSONNode resp)
            {
                if (code == 200)
                {
                    OnServerResponse(resp);
                }

                action.Invoke(code == 200);
            }

            HTTPClient.Instance.Post("bountyshop/purchase/item", CreateJson(itemId), Callback);
        }


        public void PurchaseArmouryItem(string itemId, UnityAction<bool> action)
        {
            void Callback(long code, JSONNode resp)
            {
                if (code == 200)
                {
                    App.Data.Armoury.User.UpdateWithJSON(resp["userArmouryItems"]);

                    OnServerResponse(resp);
                }

                action.Invoke(code == 200);
            }

            HTTPClient.Instance.Post("bountyshop/purchase/armouryitem", CreateJson(itemId), Callback);
        }


        // = = = Helper = = = //
        JSONNode CreateJson(string itemId)
        {
            JSONNode node = new JSONObject();

            node["shopItem"] = itemId;

            return node;
        }


        // = = = Callbacks = = = //
        void OnServerResponse(JSONNode resp)
        {
            App.Data.Inv.UpdateCurrenciesWithJSON(resp["userItems"]);

            SetDailyPurchases(resp["dailyPurchases"]);
        }
    }
}