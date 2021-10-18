using System.Collections.Generic;

namespace GM.BountyShop.Models
{
    public class CompleteBountyShopDataModel
    {
        public Dictionary<string, int> Purchases;
        public BountyShopItemsDataModel ShopItems;
    }

    public class BountyShopItemsDataModel
    {
        public List<Data.BountyShopArmouryItem> ArmouryItems;
    }
}
