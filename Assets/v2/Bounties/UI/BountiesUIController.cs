using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Bounties.UI
{
    public class BountiesUIController : GM.Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject BountySlotObject;

        [Space]
        public TMP_Text ClaimAmountText;
        [Space]
        public Slider ClaimSlider;
        public Image ClaimSliderFill;
        public Transform BountySlotParent;

        Dictionary<int, BountySlot> slots = new Dictionary<int, BountySlot>();

        void Awake()
        {
            UpdateBountySlots();
        }

        void Start()
        {
            StartCoroutine(_InternalLoop());
        }

        IEnumerator _InternalLoop()
        {
            while (true)
            {
                UpdateClaimUI();

                yield return new WaitForSecondsRealtime(0.25f);
            }
        }

        void UpdateBountySlots()
        {
            var unlockedBounties = App.Bounties.UnlockedBountiesList;

            for (int i = 0; i < unlockedBounties.Count; i++)
            {
                var bounty = unlockedBounties[i];

                if (!slots.TryGetValue(bounty.Id, out BountySlot slot))
                {
                    slot = slots[bounty.Id] = Instantiate<BountySlot>(BountySlotObject, BountySlotParent);

                    slot.Assign(bounty.Id);
                }

                slot.transform.SetSiblingIndex(i);
            }
        }

        void UpdateClaimUI()
        {
            ClaimAmountText.text = $"{App.Bounties.TotalUnclaimedPoints}/{App.Bounties.MaxClaimPoints}";
            ClaimSlider.value = App.Bounties.ClaimPercentFilled;
        }

        public void OnClaimButton()
        {
            App.Bounties.ClaimPoints((success, resp) =>
            {
                UpdateClaimUI();
            });
        }
    }
}
