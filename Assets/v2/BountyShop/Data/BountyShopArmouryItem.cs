using Newtonsoft.Json;
using UnityEngine;

namespace GM.BountyShop.Data
{
    public class BountyShopArmouryItem : BountyShopItem
    {
        public int ArmouryItemId;

        [JsonIgnore]
        public Sprite Icon => Item.Icon;

        [JsonIgnore]
        public string ItemName => Item.Name;

        [JsonIgnore]
        public Armoury.Models.ArmouryItemGameDataModel Item => App.Armoury.GetGameItem(ArmouryItemId);
    }
}
