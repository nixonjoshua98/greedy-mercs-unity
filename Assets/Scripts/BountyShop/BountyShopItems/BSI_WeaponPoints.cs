using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GreedyMercs.BountyShop.UI
{
    using GreedyMercs.BountyShop.Data;

    public class BSI_WeaponPoints : BountyShopItem
    {
        protected override void Awake()
        {
            item = BountyShopItemID.WEAPON_POINTS;
        }

        protected override void ProcessBoughtItem(JSONNode node)
        {
            GameState.Player.weaponPoints += node["weaponPointsReceived"].AsLong;
        }

        protected override string GetDescription()
        {
            BountyShopItemSO data = StaticData.BountyShop.GetItem(item);

            return string.Format("{0} Weapon Points", data.GetLong("weaponPoints"));
        }
    }
}
