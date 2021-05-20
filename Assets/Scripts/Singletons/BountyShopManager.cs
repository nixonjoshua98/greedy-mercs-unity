using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace GM.BountyShop
{
    using GM.Inventory;

    using SimpleJSON;

    public class BountyShopManager
    {
        static BountyShopManager _Instance = null;

        public static BountyShopManager Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new BountyShopManager();

                return _Instance;
            }
        }

        public BountyShopManager()
        {

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
                }

                action.Invoke(code == 200);
            }

            JSONNode node = GreedyMercs.Utils.Json.GetDeviceInfo();

            node["itemId"] = itemId;

            Server.Put("bountyshop", "purchaseItem", node, Callback);
        }
    }
}