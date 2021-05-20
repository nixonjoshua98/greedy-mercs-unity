using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM.BountyShop
{
    public class BSItemsSection : MonoBehaviour
    {
        [SerializeField] Text shopRefreshText;

        public BountyShopSlot slot;

        void Start()
        {
            slot.SetItem(GreedyMercs.StaticData.BountyShop.GetItem(0));
        }

        void OnEnable()
        {
            BountyShopManager.Instance.Refresh(OnShopRefreshed);
        }

        void OnShopRefreshed()
        {
            Debug.Log("OnShopRefreshed");
        }

        void FixedUpdate()
        {
            shopRefreshText.text = "Refreshes in ???";
        }
    }
}