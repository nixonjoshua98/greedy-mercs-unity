using UnityEngine;

namespace GM.BountyShop.Data
{
    public class BountyShopArmouryItemData : BountyShopItem
    {
        public int ArmouryItemID;

        public Sprite Icon => ItemData.Icon;
        public int ItemTier => ItemData.Tier;
        public string ItemName => ItemData.Name;

        Armoury.Data.ArmouryItemGameData ItemData => App.Data.Armoury.Game[ArmouryItemID]; // Private
    }
}
