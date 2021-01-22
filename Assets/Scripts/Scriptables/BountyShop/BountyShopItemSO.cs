using SimpleJSON;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GreedyMercs.BountyShop.Data
{
    public enum BountyShopItemID
    {
        PRESTIGE_POINTS = 0,
        GEMS            = 1,
        WEAPON_POINTS   = 2
    }

    [PreferBinarySerialization]
    [CreateAssetMenu(menuName = "Scriptables/BountyShopItem")]
    public class BountyShopItemSO : ScriptableObject
    {
        public BountyShopItemID ShopItemID;

        [Header("Static Data")]
        public int maxResetBuy;
        public int purchaseCost;

        JSONNode node;

        public void Init(JSONNode _node)
        {
            node = _node;

            maxResetBuy = node["maxResetBuy"].AsInt;
            purchaseCost = node["purchaseCost"].AsInt;
        }

        JSONNode Get(string key)
        {
            if (!node.HasKey(key))
                Debug.Log("Bounty item is missing key " + key);

            return node[key];
        }

        public int PurchaseCost(int owned)
        {
            JSONNode purchaseData = node["purchaseData"];

            return purchaseData["baseCost"] + (purchaseData["levelIncrease"] * owned);
        }

        public float GetFloat(string key) => Get(key).AsFloat;
        public long GetLong(string key) => Get(key).AsLong;
    }
}