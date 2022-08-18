using SRC.Armoury.Data;
using SRC.BountyShop.Data;
using SRC.Common.Enums;
using SRC.Inventory;
using System.Collections.Generic;
using System.Linq;

namespace SRC.BountyShop.Requests
{
    public class PurchaseArmouryItemResponse : SRC.HTTP.Requests.ServerResponse
    {
        public UserCurrencies Currencies;
        public UserArmouryItem ArmouryItem;
    }

    public class PurchaseCurrencyResponse : SRC.HTTP.Requests.ServerResponse
    {
        public UserCurrencies Currencies;
    }

    public class GetBountyShopResponse : SRC.HTTP.Requests.ServerResponse
    {
        public int GameDayNumber;
        public List<BountyShopPurchaseModel> Purchases;
        public BountyShopItems ShopItems;

        public BountyShopPurchaseModel GetPurchase(BountyShopItemType itemType, string itemId)
        {
            return Purchases.FirstOrDefault(p => p.ItemID == itemId && p.ItemType == itemType);
        }
    }
}