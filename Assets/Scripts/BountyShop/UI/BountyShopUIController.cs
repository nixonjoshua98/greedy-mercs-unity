using GM.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GM.BountyShop.UI
{
    public class BountyShopUIController : GM.Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] GameObject ArmouryItemSlotObject;
        [SerializeField] GameObject CurrencyItemSlotObject;

        [Header("References")]
        [SerializeField] Transform ItemsParent;
        [SerializeField] TMP_Text RefreshText;
        [SerializeField] TypeWriter LoadingTypeWriter;        
        
        List<GameObject> ItemSlotObjects = new();

        void Awake()
        {
            App.BountyShop.E_ShopUpdated.AddListener(OnShopUpdated);
        }

        void Start()
        {
            StartCoroutine(_InternalLoop());
        }

        void OnShopUpdated()
        {
            InstantiateItemSlots();
        }

        void OnClientOffline()
        {
            RefreshText.text = "Offline";
            LoadingTypeWriter.enabled = false;

            ShowDefaultItemSlots();
        }

        void ShowDefaultItemSlots()
        {
            ItemSlotObjects.ForEach(x => Destroy(x));
        }

        IEnumerator _InternalLoop()
        {
            bool isPrevValid = App.BountyShop.IsValid;

            while (!App.HTTP.IsOffline)
            {
                LoadingTypeWriter.enabled = !App.BountyShop.IsValid;

                if (App.BountyShop.IsValid)
                {
                    RefreshText.text = $"Time Until Refresh\n<color=orange>{App.DailyRefresh.TimeUntilNext.ToString(TimeSpanFormat.Default)}</color>";
                }
                // Shop has invalidated - Most likely is being refreshed
                else if (!App.BountyShop.IsValid && isPrevValid)
                {
                    ShowDefaultItemSlots();
                }

                isPrevValid = App.BountyShop.IsValid;

                yield return new WaitForSecondsRealtime(1.0f);
            }

            OnClientOffline();
        }

        T InstantiateItemSlot<T>(GameObject prefab) where T: MonoBehaviour
        {
            var slot = this.Instantiate<T>(prefab, ItemsParent);

            ItemSlotObjects.Add(slot.gameObject);

            return slot;
        }

        void RandomizeSlotPositions()
        {
            var rnd = GM.Common.Utility.SeededRandom(App.DailyRefresh.Previous.ToString());

            for (int i = 0; i < ItemsParent.childCount; i++)
            {
                int val = rnd.Next(0, ItemsParent.childCount);

                ItemsParent.GetChild(i).SetSiblingIndex(val);
            }
        }

        void InstantiateItemSlots()
        {
            App.BountyShop.ArmouryItems.ForEach(item => InstantiateItemSlot<BountyShopArmouryItemSlot>(ArmouryItemSlotObject).Set(item));
            App.BountyShop.CurrencyItems.ForEach(item => InstantiateItemSlot<BountyShopCurrencyItemSlot>(CurrencyItemSlotObject).Set(item));

            RandomizeSlotPositions();
        }
    }
}