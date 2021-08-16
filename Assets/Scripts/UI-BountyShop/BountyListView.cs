using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Bounty.UI
{
    using GM.UI;

    public class BountyListView : CloseablePanel
    {
        [SerializeField] GameObject BountyObjectSlot;

        [Space]

        [SerializeField] GameObject bountyListParent;

        List<BountySlot> bountySlots;

        void Awake()
        {
            bountySlots = new List<BountySlot>();
        }


        void Start()
        {
            ReInstantiateIcons();
        }

        protected override void OnShown()
        {
            InstantiateIcons();
        }


        protected override void OnHidden()
        {
            DestroyBountySlots();
        }


        void ReInstantiateIcons()
        {
            DestroyBountySlots();
            InstantiateIcons();
        }

        void InstantiateIcons()
        {
            foreach (BountyState bounty in UserData.Get.Bounties.StatesList)
            {
                BountySlot slot = CanvasUtils.Instantiate<BountySlot>(BountyObjectSlot, bountyListParent);

                slot.SetBounty(bounty.ID);

                bountySlots.Add(slot);
            }
        }

        void DestroyBountySlots()
        {
            foreach (BountySlot slot in bountySlots)
            {
                Destroy(slot.gameObject);
            }

            bountySlots.Clear();
        }
    }
}
