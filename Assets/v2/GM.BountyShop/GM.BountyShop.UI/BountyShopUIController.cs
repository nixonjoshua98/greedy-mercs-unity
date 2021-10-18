using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

namespace GM.BountyShop.UI
{
    public class BountyShopUIController : Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject ItemSlotObject;

        [Header("References")]
        public TMP_Text RefreshText;
        public Transform ItemsParent;

        void Awake()
        {
            InstantiateArmouryItems();
        }

        void InstantiateArmouryItems()
        {
            foreach (var item in App.Data.BountyShop.ArmouryItems)
            {
                Instantiate<BSArmouryItemSlot>(ItemSlotObject, ItemsParent)
                    .AssignShopItem(item);
            }
        }

        void FixedUpdate()
        {
            RefreshText.text = $"Daily Bounty Shop | <color=orange>{App.Data.TimeUntilDailyReset.Format()}</color>";
        }
    }
}