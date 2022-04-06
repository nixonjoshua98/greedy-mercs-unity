using Newtonsoft.Json;

namespace GM.BountyShop.Data
{
    public class BountyShopArmouryItem : BountyShopItem
    {
        public int ItemID;

        [JsonIgnore]
        public Armoury.AggregatedArmouryItem Item => App.Armoury.GetItem(ItemID);
    }
}
