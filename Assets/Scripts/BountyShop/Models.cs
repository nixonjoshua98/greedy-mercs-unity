using System.Collections.Generic;

namespace GM.BountyShop.Data
{
    public class BountyShopPurchaseModel
    {
        public string ItemID;
    }

    public class BountyShopItems
    {
        public List<BountyShopArmouryItem> ArmouryItems;
        public List<BountyShopCurrencyItem> CurrencyItems;
    }

    public class UserBountyShop
    {
        public List<BountyShopPurchaseModel> Purchases;
        public BountyShopItems ShopItems;
    }
}
