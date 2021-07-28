
using System.Collections.Generic;

using UnityEngine.Events;

namespace GM.BountyShop
{
    using GM.Server;

    using SimpleJSON;

    struct BountyShopPurchaseData
    {
        public int TotalDailyPurchases;

        public BountyShopPurchaseData(int total)
        {
            TotalDailyPurchases = total;
        }
    }


    public class UserBountyShop
    {
        Dictionary<string, BountyShopPurchaseData> purchases;

        public ServerBountyShopData ServerData;


        public UserBountyShop(JSONNode node)
        {
            SetDailyPurchases(node["dailyPurchases"]);

            ServerData = new ServerBountyShopData();

            Refresh(() => { });
        }


        public void SetDailyPurchases(JSONNode node)
        {
            purchases = new Dictionary<string, BountyShopPurchaseData>();

            // {0: 0, 1: 3}
            foreach (string key in node.Keys)
            {
                BountyShopPurchaseData inst = new BountyShopPurchaseData(node[key].AsInt);

                purchases.Add(key, inst);
            }
        }


        // = = = Quick Helper = = = //

        public bool InStock(string itemId)
        {
            int dailyPurchases = 0;

            if (purchases.TryGetValue(itemId, out BountyShopPurchaseData data))
                dailyPurchases = data.TotalDailyPurchases;

            return ServerData.Get(itemId).DailyPurchaseLimit > dailyPurchases;
        }


        // = = = Server Methods ===
        public void Refresh(UnityAction action)
        {
            void InternalCallback(long code, JSONNode resp)
            {
                if (code == 200)
                {
                    // Update the store items pulled from the server
                    ServerData.UpdateAll(resp["bountyShopItems"]);

                    // Updates the next daily reset time
                    StaticData.NextDailyReset = Funcs.ToDateTime(resp["nextDailyResetTime"].AsLong);

                    OnServerResponse(resp);

                    action.Invoke();
                }
            }

            HTTPClient.Get().Post("bountyshop/refresh", InternalCallback);
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

            HTTPClient.Get().Post("bountyshop/purchase/item", CreateJson(itemId), Callback);
        }

        public void PurchaseArmouryItem(string itemId, UnityAction<bool> action)
        {
            void Callback(long code, JSONNode resp)
            {
                if (code == 200)
                {
                    UserData.Get().Armoury.SetArmouryItems(resp["userArmouryItems"]);

                    OnServerResponse(resp);
                }

                action.Invoke(code == 200);
            }

            HTTPClient.Get().Post("bountyshop/purchase/armouryitem", CreateJson(itemId), Callback);
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
            UserData.Get().Inventory.SetItems(resp["userItems"]);

            SetDailyPurchases(resp["dailyPurchases"]);
        }
    }
}