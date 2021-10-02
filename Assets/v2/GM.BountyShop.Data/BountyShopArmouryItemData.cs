using UnityEngine;

namespace GM.BountyShop.Data
{
    public class BountyShopArmouryItemData : BountyShopItem
    {
        public Armoury.Data.ArmouryItemGameData Item => App.Data.Armoury.Game[ArmouryItemID];
        public int ArmouryItemID;
        public Sprite Icon => Item.Icon;
    }
}
