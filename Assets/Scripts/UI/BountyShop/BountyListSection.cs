using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Bounty.UI
{
    public class BountyListSection : MonoBehaviour
    {
        [SerializeField] GameObject BountyObjectSlot;
        [Space]
        [SerializeField] GameObject bountyListParent;

        List<BountySlot> bountySlots;

        void Awake()
        {
            bountySlots = new List<BountySlot>();

            UpdateInterface();
        }


        void UpdateInterface()
        {
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
    }
}
