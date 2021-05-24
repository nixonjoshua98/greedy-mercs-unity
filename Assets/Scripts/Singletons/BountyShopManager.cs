using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace GM.BountyShop
{
    using GM.Armoury;
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

        public ServerBountyShopData ServerData;

        public static BountyShopManager Create(JSONNode node)
        {
            Instance = new BountyShopManager(node);

            return Instance;
        }

        BountyShopManager(JSONNode node)
        {
            SetDailyPurchases(node["dailyPurchases"]);

            ServerData = new ServerBountyShopData();

            Refresh(() => { });
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

            return ServerData.Get(iid).DailyPurchaseLimit > dailyPurchases;
        }

        // = = = Server Methods ===
        public void Refresh(UnityAction action)
        {
            void Callback(long code, string body)
            {
                if (code == 200)
                {
                    JSONNode resp = GreedyMercs.Utils.Json.Decompress(body);

                    // Update the store items pulled from the server
                    ServerData.UpdateAll(resp["serverData"]);
                       
                    // Updates the next daily reset time
                    GreedyMercs.StaticData.NextDailyReset = Funcs.ToDateTime(resp["nextDailyResetTime"].AsLong);

                    // Updates the users purchases
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

                    OnPurchaseAnyItem(itemId, resp);
                }

                action.Invoke(code == 200);
            }

            Server.Put("bountyshop", "purchaseItem", CreateJson(itemId), Callback);
        }

        public void PurchaseArmouryItem(int itemId, UnityAction<bool> action)
        {
            void Callback(long code, string body)
            {
                if (code == 200)
                {
                    JSONNode resp = GreedyMercs.Utils.Json.Decompress(body);

                    ArmouryManager.Instance.SetArmouryItems(resp["userArmouryItems"]);

                    OnPurchaseAnyItem(itemId, resp);
                }

                action.Invoke(code == 200);
            }

            Server.Put("bountyshop", "purchaseArmouryItem", CreateJson(itemId), Callback);
        }

        // = = = Helper = = =
        JSONNode CreateJson(int itemId)
        {
            JSONNode node = GreedyMercs.Utils.Json.GetDeviceInfo();

            node["itemId"] = itemId;

            return node;
        }

        // = = = Callbacks = = =

        void OnPurchaseAnyItem(int itemId, JSONNode resp)
        {
            JSONNode userItems = resp["userItems"];

            InventoryManager.Instance.SetItems(userItems);

            Refresh(() => { }); // Temp
        }
    }
}