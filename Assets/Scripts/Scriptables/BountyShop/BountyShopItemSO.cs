﻿using SimpleJSON;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GreedyMercs.Data.BountyShop
{
    public enum BountyShopItemID
    {
        PRESTIGE_90 = 0
    }

    [PreferBinarySerialization]
    [CreateAssetMenu(menuName = "Scriptables/BountyShopItem")]
    public class BountyShopItemSO : ScriptableObject
    {
        public BountyShopItemID ShopItemID;

        [Header("Static Data")]
        public int maxResetBuy;

        public void Init(JSONNode node)
        {
            maxResetBuy = node["maxResetBuy"].AsInt;
        }
    }
}