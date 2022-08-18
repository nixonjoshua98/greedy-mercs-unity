using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SRC.Bounties.UI
{
    public class BountiesUIController : SRC.Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject BountyListObject;
        [SerializeField] private GameObject BountyShopPanelObject;

        [Header("Transforms")]
        [SerializeField] private Transform BountySlotParent;

        [Header("Text Elements")]
        [SerializeField] private TMP_Text RefreshTimeText;

        private readonly Dictionary<int, BountyListSlot> slots = new();

        private void FixedUpdate()
        {
            UpdateSlotsUI();
        }

        private void UpdateSlotsUI()
        {
            var unlockedBounties = App.Bounties.UnlockedBounties;

            for (int i = 0; i < unlockedBounties.Count; i++)
            {
                var bounty = unlockedBounties[i];

                if (!slots.TryGetValue(bounty.BountyID, out var slot))
                {
                    slot = slots[bounty.BountyID] = this.Instantiate<BountyListSlot>(BountyListObject, BountySlotParent);

                    slot.Initialize(bounty);
                }

                slot.transform.SetSiblingIndex(i);
            }
        }

        public void ShowBountyShop()
        {
            UI.Instantiate(BountyShopPanelObject, SRC.UI.UILayer.TWO);
        }

        public void OnClaimButton()
        {
            App.Bounties.ClaimPoints((success, resp) =>
            {
                if (!success)
                    GMLogger.Error(resp.Message);
            });
        }
    }
}