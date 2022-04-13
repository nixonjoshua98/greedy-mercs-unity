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
}
