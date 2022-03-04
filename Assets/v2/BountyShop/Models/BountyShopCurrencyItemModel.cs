using GM.BountyShop.Data;
using GM.Common.Enums;
using Newtonsoft.Json;
using GM.CurrencyItems.ScriptableObjects;

namespace GM.BountyShop.Models
{
    public class BountyShopCurrencyItemModel : BountyShopItem
    {
        public CurrencyType CurrencyType;

        public long QuantityPerPurchase;

        [JsonIgnore]
        public CurrencyItemScriptableObject Item => App.GMData.Items.GetItem(CurrencyType);
    }
}