using System.Collections;
using System.Collections.Generic;

using SimpleJSON;
using UnityEngine;

namespace GreedyMercs.BountyShop.Data
{
    [PreferBinarySerialization]
    [CreateAssetMenu(menuName = "Scriptables/Containers/BountyShopList")]
    public class BountyShopListSO : ScriptableObject
    {
        [SerializeField] BountyShopItemSO[] items;

        Dictionary<BountyShopItemID, BountyShopItemSO> itemsDict;

        public void Init(JSONNode node)
        {
            itemsDict = new Dictionary<BountyShopItemID, BountyShopItemSO>();

            foreach (BountyShopItemSO item in items)
            {
                item.Init(node[((int)item.ShopItemID).ToString()]);

                itemsDict[item.ShopItemID] = item;
            }
        }

        public BountyShopItemSO GetItem(BountyShopItemID key) => itemsDict[key];
    }
}