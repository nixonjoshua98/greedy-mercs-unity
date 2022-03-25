using Newtonsoft.Json;
using UnityEngine;

namespace GM.BountyShop.Data
{
    public class BountyShopArmouryItem : BountyShopItem
    {
        public int ItemID;

        [JsonIgnore]
        public Sprite Icon => Item.Icon;

        [JsonIgnore]
        public string ItemName => Item.Name;

        [JsonIgnore]
        public Armoury.Models.ArmouryItem Item => App.Armoury.GetGameItem(ItemID);
    }
}
