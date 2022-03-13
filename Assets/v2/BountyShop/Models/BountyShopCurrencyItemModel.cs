using GM.BountyShop.Data;
using GM.Common.Enums;
using GM.CurrencyItems.ScriptableObjects;
using Newtonsoft.Json;

namespace GM.BountyShop.Models
{
    public class BountyShopCurrencyItemModel : BountyShopItem
    {
        public CurrencyType CurrencyType;

        public long QuantityPerPurchase;

        [JsonIgnore]
        public CurrencyItemScriptableObject Item => App.Items.GetItem(CurrencyType);
    }
}