using UnityEngine;
using Newtonsoft.Json;

namespace GM.BountyShop.Data
{
    public class BountyShopArmouryItem : BountyShopItem
    {
        public int ArmouryItem;

        [JsonIgnore]
        public Sprite Icon => Item.Icon;

        [JsonIgnore]
        public int ItemTier => Item.Tier;

        [JsonIgnore]
        public string ItemName => Item.Name;

        [JsonIgnore]
        public Armoury.Models.ArmouryItemGameDataModel Item => App.Data.Armoury.GetGameItem(ArmouryItem);
    }
}
