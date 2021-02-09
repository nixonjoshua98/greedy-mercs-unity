using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs.BountyShop.UI
{
    using GreedyMercs.Armoury.Data;

    using GreedyMercs.Armoury.UI;

    public class ArmouryChestPanel : MonoBehaviour
    {
        [SerializeField] ArmouryItemSlot itemSlot;

        [Header("References")]
        [SerializeField] GameObject claimButtonObject;

        public void Init(int itemReceived)
        {
            claimButtonObject.SetActive(false);

            StartCoroutine(Animation(itemReceived));
        }

        IEnumerator Animation(int itemReceived)
        {
            List<ArmouryItemSO> items = StaticData.Armoury.GetAllItems();

            for (int i = 0; i < Random.Range(10, 15); ++i)
            {
                itemSlot.Init(items[Random.Range(0, items.Count)], hideLockedPanel: true);

                yield return new WaitForSeconds(0.15f);
            }

            itemSlot.Init(StaticData.Armoury.GetWeapon(itemReceived));

            yield return new WaitForSeconds(0.5f);

            claimButtonObject.SetActive(true);
        }

        // === Button Callback ===

        public void ClaimItem()
        {
            Destroy(gameObject);
        }
    }
}