using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        [Newtonsoft.Json.JsonIgnore]
        public List<Data.BountyShopCurrencyItemData> CurrencyItems = new List<Data.BountyShopCurrencyItemData>();
    }
}
