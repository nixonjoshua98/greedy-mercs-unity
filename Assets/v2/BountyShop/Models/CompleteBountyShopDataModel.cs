using System.Collections.Generic;
using GM.BountyShop.Data;

namespace GM.BountyShop.Models
{
    public class BountyShopItemsModel
    {
        public List<BountyShopArmouryItem> ArmouryItems;
        public List<BountyShopCurrencyItemModel> CurrencyItems;
    }

    public class CompleteBountyShopDataModel
    {
        public List<BountyShopPurchaseModel> Purchases;
        public BountyShopItemsModel ShopItems;
    }
}
