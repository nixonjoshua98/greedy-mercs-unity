using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.BountyShop.UI
{
    public class BountyShopUIController : GM.UI.PanelController
    {
        [Header("Prefabs")]
        public GameObject ItemSlotObject;

        [Header("References")]
        public Transform ItemsParent;

        void Awake()
        {
            InstantiateArmouryItems();
        }

        void InstantiateArmouryItems()
        {
            foreach (var item in App.Data.BountyShop.ArmouryItems)
            {
                Instantiate(ItemSlotObject, ItemsParent);
            }
        }
    }
}