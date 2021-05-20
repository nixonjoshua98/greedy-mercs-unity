using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace GM.BountyShop
{
    using GM.Inventory;

    using SimpleJSON;

    struct BountyShopPurchaseData
    {
        public int TotalDailyPurchases;

        public BountyShopPurchaseData(int total)
        {
            TotalDailyPurchases = total;
        }
    }


    public class BountyShopManager
    {
        public static BountyShopManager Instance = null;

        Dictionary<int, BountyShopPurchaseData> purchases;

        public static BountyShopManager Create(JSONNode node)
        {
            Instance = new BountyShopManager();

            Instance.SetDailyPurchases(node["dailyPurchases"]);

            return Instance;
        }

        public void SetDailyPurchases(JSONNode node)
        {
            purchases = new Dictionary<int, BountyShopPurchaseData>();

            // {0: 0, 1: 3}
            foreach (string key in node.Keys)
            {
                int id = int.Parse(key);

                BountyShopPurchaseData inst = new BountyShopPurchaseData(node[key].AsInt);

                purchases.Add(id, inst);
            }
        }

        // = = = Quick Helper = = =
        public bool InStock(int iid)
        {
            int dailyPurchases = 0;

            if (purchases.TryGetValue(iid, out BountyShopPurchaseData data))
                dailyPurchases = data.TotalDailyPurchases;

            return GreedyMercs.StaticData.BountyShop.GetItem(iid).DailyPurchaseLimit > dailyPurchases;
        }

        // = = = Server Methods ===
        public void Refresh(UnityAction action)
        {
            void Callback(long code, string body)
            {
                if (code == 200)
                {
                    JSONNode resp = GreedyMercs.Utils.Json.Decompress(body);

                    GreedyMercs.StaticData.BountyShop.UpdateAll(resp["serverData"]);

                    SetDailyPurchases(resp["dailyPurchases"]);

                    action.Invoke();
                }
            }

            JSONNode node = GreedyMercs.Utils.Json.GetDeviceInfo();

            Server.Put("bountyshop", "refreshShop", node, Callback);
        }

        public void PurchaseItem(int itemId, UnityAction<bool> action)
        {
            void Callback(long code, string body)
            {
                if (code == 200)
                {
                    JSONNode resp = GreedyMercs.Utils.Json.Decompress(body);

                    JSONNode userItems = resp["userItems"];

                    InventoryManager.Instance.SetItems(userItems);

                    Refresh(() => { }); // Temp
                }

                action.Invoke(code == 200);
            }

            JSONNode node = GreedyMercs.Utils.Json.GetDeviceInfo();

            node["itemId"] = itemId;

            Server.Put("bountyshop", "purchaseItem", node, Callback);
        }
    }
}