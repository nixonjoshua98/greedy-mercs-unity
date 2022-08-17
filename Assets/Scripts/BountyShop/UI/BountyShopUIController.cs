using SRC.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SRC.BountyShop.UI
{
    public class BountyShopUIController : SRC.Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject ArmouryItemSlotObject;
        [SerializeField] private GameObject CurrencyItemSlotObject;

        [Header("References")]
        [SerializeField] private Transform ItemsParent;
        [SerializeField] private TMP_Text RefreshText;
        [SerializeField] private TypeWriter LoadingTypeWriter;
        private readonly List<GameObject> ItemSlotObjects = new();
        private bool _IsShopShown = false;

        private void Start()
        {
            StartCoroutine(_InternalLoop());
        }

        private void OnShopUpdated()
        {
            InstantiateItemSlots();
        }

        private void OnClientOffline()
        {
            RefreshText.text = "Offline";
            LoadingTypeWriter.enabled = false;

            DestroyItemSlots();
        }

        private void DestroyItemSlots()
        {
            _IsShopShown = false;

            ItemSlotObjects.ForEach(x => Destroy(x));
        }

        private IEnumerator _InternalLoop()
        {
            bool isPrevValid = App.BountyShop.IsValid;

            while (!App.HTTP.IsOffline)
            {
                LoadingTypeWriter.enabled = !App.BountyShop.IsValid;

                if (App.BountyShop.IsValid)
                {
                    RefreshText.text = $"Time Until Refresh\n<color=orange>{App.DailyRefresh.TimeUntilNext.ToString(TimeSpanFormat.Default)}</color>";

                    if (!_IsShopShown)
                    {
                        InstantiateItemSlots();
                    }
                }

                // Shop has invalidated - Most likely is being refreshed
                else if (!App.BountyShop.IsValid && isPrevValid)
                {
                    DestroyItemSlots();
                }

                isPrevValid = App.BountyShop.IsValid;

                yield return new WaitForSecondsRealtime(1.0f);
            }

            OnClientOffline();
        }

        private T InstantiateItemSlot<T>(GameObject prefab) where T : MonoBehaviour
        {
            var slot = this.Instantiate<T>(prefab, ItemsParent);

            ItemSlotObjects.Add(slot.gameObject);

            return slot;
        }

        private void RandomizeSlotPositions()
        {
            var rnd = SRC.Common.Utility.SeededRandom(App.DailyRefresh.Previous);

            for (int i = 0; i < ItemsParent.childCount; i++)
            {
                int val = rnd.Next(0, ItemsParent.childCount);

                ItemsParent.GetChild(i).SetSiblingIndex(val);
            }
        }

        private void InstantiateItemSlots()
        {
            _IsShopShown = true;

            App.BountyShop.ArmouryItems.ForEach(item => InstantiateItemSlot<BountyShopArmouryItemSlot>(ArmouryItemSlotObject).Set(item));
            App.BountyShop.CurrencyItems.ForEach(item => InstantiateItemSlot<BountyShopCurrencyItemSlot>(CurrencyItemSlotObject).Set(item));

            RandomizeSlotPositions();
        }
    }
}