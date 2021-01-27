using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;

namespace GreedyMercs.BountyShop.UI
{
    using GreedyMercs.BountyShop.Data;
    using GreedyMercs.Armoury.Data;

    public class BSI_Weapon : BountyShopItem
    {
        protected override void Awake()
        {
            item = BountyShopItemID.WEAPON;
        }

        protected override void ProcessBoughtItem(JSONNode node)
        {
            ArmouryWeaponState state = GameState.Armoury.GetWeapon(node["weaponReceived"].AsInt);

            state.level++;
        }

        protected override string GetDescription()
        {
            return "1 Random Weapon";
        }
    }
}