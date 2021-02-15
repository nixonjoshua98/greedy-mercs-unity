using SimpleJSON;

namespace GreedyMercs.BountyShop.Data
{
    public class BountyShopItemData
    {
        JSONNode node;

        public readonly int dailyPurchaseLimit;

        public BountyShopItemData(JSONNode _node)
        {
            node = _node;

            dailyPurchaseLimit  = node["dailyPurchaseLimit"].AsInt;
        }

        public int PurchaseCost(int purchased)
        {
            JSONNode purchaseData = node["purchaseData"];

            return purchaseData["baseCost"].AsInt + (purchaseData["purchaseIncrease"].AsInt * purchased);
        }

        public int GetInt(string key) => node[key].AsInt;
        public float GetFloat(string key) => node[key].AsFloat;
    }
}