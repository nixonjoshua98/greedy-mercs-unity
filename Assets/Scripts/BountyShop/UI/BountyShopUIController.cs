using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using System.Collections;

namespace GM.BountyShop.UI
{
    public class BountyShopUIController : GM.Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject ArmouryItemSlotObject;
        public GameObject CurrencyItemSlotObject;

        [Header("References")]
        public Transform ItemsParent;
        [SerializeField] TMP_Text RefreshText;

        private void Awake()
        {
            InstantiateItemSlots();
        }

        void Start()
        {
            StartCoroutine(_InternalLoop());
        }

        IEnumerator _InternalLoop()
        {
            while (true)
            {
                RefreshText.text = $"items refresh in <color=orange>{ App.DailyRefresh.TimeUntilNext.Format() }</color>";

                yield return new WaitForSecondsRealtime(0.25f);
            }
        }

        private void InstantiateItemSlots()
        {
            App.BountyShop.CurrencyItems.ForEach(item =>
            {
                Instantiate<BountyShopCurrencyItemSlot>(CurrencyItemSlotObject, ItemsParent).Set(item);
            });

            App.BountyShop.ArmouryItems.ForEach(item =>
            {
                Instantiate<BountyShopArmouryItemSlot>(ArmouryItemSlotObject, ItemsParent).Set(item);
            });

            var rnd = GM.Common.Utility.SeededRandom(App.DailyRefresh.Previous.ToString());

            for (int i = 0; i < ItemsParent.childCount; i++)
            {
                int val = rnd.Next(0, ItemsParent.childCount);

                ItemsParent.GetChild(i).SetSiblingIndex(val);
            }

        }
    }
}