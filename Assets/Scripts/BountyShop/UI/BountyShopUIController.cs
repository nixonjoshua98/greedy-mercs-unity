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
        public GameObject ArmouryItemSlotObject;
        public GameObject CurrencyItemSlotObject;

        [Header("References")]
        public Transform ItemsParent;
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

        IEnumerator _InternalLoop()
        {
            while (true)
            {
                LoadingTypeWriter.enabled = !App.BountyShop.IsValid && !App.HTTP.IsOffline;

                if (App.HTTP.IsOffline)
                {
                    RefreshText.text = "Bounty Shop Offline";

                    ItemSlotObjects.ForEach(x => Destroy(x));
                }
                else if (App.BountyShop.IsValid)
                {
                    RefreshText.text = $"items refresh in <color=orange>{App.DailyRefresh.TimeUntilNext.Format()}</color>";
                }
                else
                {
                    ItemSlotObjects.ForEach(x => Destroy(x));
                }

                yield return new WaitForSecondsRealtime(1.0f);
            }
        }

        void CreateCurrencyItemSlots()
        {
            App.BountyShop.CurrencyItems.ForEach(item =>
            {
                var slot = Instantiate<BountyShopCurrencyItemSlot>(CurrencyItemSlotObject, ItemsParent);

                ItemSlotObjects.Add(slot.gameObject);

                slot.Set(item);
            });
        }

        void CreateArmouryItemSlots()
        {
            App.BountyShop.ArmouryItems.ForEach(item =>
            {
                var slot = Instantiate<BountyShopArmouryItemSlot>(ArmouryItemSlotObject, ItemsParent);

                ItemSlotObjects.Add(slot.gameObject);

                slot.Set(item);
            });
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
            CreateCurrencyItemSlots();
            CreateArmouryItemSlots();
            RandomizeSlotPositions();
        }
    }
}