using System.Collections.Generic;
using UnityEngine;

namespace SRC.Bounties.UI
{
    public class BountiesUIController : SRC.Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] GameObject BountyListObject;

        [Header("Transforms")]
        [SerializeField] Transform BountySlotParent;

        private readonly Dictionary<int, BountyListSlot> slots = new();

        void FixedUpdate()
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