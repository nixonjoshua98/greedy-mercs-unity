using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;

namespace GreedyMercs.BountyShop.Data
{
    public class BountyItemState
    {
        public int dailyPurchased;
    }

    public class PlayerBountyShopData
    {
        Dictionary<BountyShopItemID, BountyItemState> items;

        public DateTime lastReset;

        public bool IsValid { get { return lastReset >= GameState.LastDailyReset; } }

        public PlayerBountyShopData(JSONNode node)
        {
            Update(node);
        }

        public void Update(JSONNode node)
        {
            items = new Dictionary<BountyShopItemID, BountyItemState>();

            foreach (string key in node["itemsBought"].Keys)
            {
                BountyShopItemID itemId = (BountyShopItemID)int.Parse(key);

                items[itemId] = new BountyItemState { dailyPurchased = node["itemsBought"][key].AsInt };
            }

            lastReset = DateTimeOffset.FromUnixTimeMilliseconds(node["lastReset"].AsLong).DateTime;
        }

        public JSONNode ToJson()
        {
            JSONNode node           = new JSONObject();
            JSONNode itemsBought    = new JSONObject();

            foreach (var entry in items)
            { 
                itemsBought[((int)entry.Key).ToString()] = entry.Value.dailyPurchased.ToString();
            }

            node.Add("lastReset", lastReset.ToUnixMilliseconds());

            node.Add("itemsBought", itemsBought);

            return node;
        }


        public BountyItemState GetItem(BountyShopItemID key)
        {
            if (items.TryGetValue(key, out BountyItemState item))
                return item;

            items[key] = new BountyItemState { dailyPurchased = 0 };

            return GetItem(key);
        }

        // # ===Helper Methods === #

        public bool IsItemMaxBought(BountyShopItemID key)
        {
            BountyShopItemSO data = StaticData.BountyShop.GetItem(key);

            return GetItem(key).dailyPurchased >= data.maxResetBuy;
        }

        public bool CanAffordItem(BountyShopItemID key)
        {
            BountyItemState state = GetItem(key);

            BountyShopItemSO data = StaticData.BountyShop.GetItem(key);

            return GameState.Player.bountyPoints >= data.PurchaseCost(state.dailyPurchased);
        }

        public void ProcessPurchase(BountyShopItemID key)
        {
            BountyItemState state = GetItem(key);

            BountyShopItemSO data = StaticData.BountyShop.GetItem(key);

            GameState.Player.bountyPoints -= data.PurchaseCost(state.dailyPurchased);

            state.dailyPurchased++;
        }
    }
}
