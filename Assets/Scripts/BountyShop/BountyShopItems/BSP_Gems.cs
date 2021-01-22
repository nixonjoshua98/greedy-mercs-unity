using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GreedyMercs.BountyShop.UI
{
    using GreedyMercs.BountyShop.Data;
    using SimpleJSON;

    public class BSP_Gems : BountyShopItem
    {
        void Awake()
        {
            item = BountyShopItemID.GEMS;
        }

        protected override void ProcessBoughtItem(JSONNode node)
        {
            GameState.Player.gems += node["receivedGems"].AsLong;
        }

        protected override string GetDescription()
        {
            BountyShopItemSO data = StaticData.BountyShop.GetItem(item);

            return string.Format("{0} Gems", data.GetLong("gemsGiven"));
        }
    }
}
