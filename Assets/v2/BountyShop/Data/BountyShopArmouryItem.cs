using UnityEngine;
using Newtonsoft.Json;

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
        public Armoury.Models.ArmouryItemGameDataModel Item => App.DataContainers.Armoury.GetGameItem(ArmouryItemId);
    }
}
