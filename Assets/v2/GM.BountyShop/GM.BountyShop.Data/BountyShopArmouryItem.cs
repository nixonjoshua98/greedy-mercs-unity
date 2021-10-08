using UnityEngine;
using Newtonsoft.Json;

namespace GM.BountyShop.Data
{
    public class BountyShopArmouryItem : BountyShopItem
    {
        public int ArmouryItem;

        [JsonIgnore]
        public Sprite Icon => ItemData.Icon;

        [JsonIgnore]
        public int ItemTier => ItemData.Tier;

        [JsonIgnore]
        public string ItemName => ItemData.Name;

        [JsonIgnore]
        Armoury.Models.ArmouryItemGameDataModel ItemData => App.Data.Armoury.Game.GetItem(ArmouryItem); // Private
    }
}
