using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM.Bounty
{
    using GM.Inventory;

    using GameState = GreedyMercs.GameState;

    public class BountyTab : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] GameObject BountyObjectSlot;

        [Header("Objects")]
        [SerializeField] Transform bountySlotsParent;

        [Header("Components")]
        [SerializeField] Text bountyIncomeText;
        [SerializeField] Text unclaimedTotalText;
        [SerializeField] Text bountyPointsText;
        [Space]
        [SerializeField] Slider bountySlider;

        void Start()
        {
            InstantiateIcons();
        }

        void FixedUpdate()
        {
            InventoryManager inv = InventoryManager.Instance;

            bountyPointsText.text   = inv.bountyPoints.ToString();
            bountyIncomeText.text   = string.Format("{0} / hour (Max {1})", BountyManager.Instance.MaxHourlyIncome, BountyManager.Instance.TotalCapacity);
            unclaimedTotalText.text = string.Format("Collect ({0})", BountyManager.Instance.UnclaimedTotal);

            bountySlider.value = BountyManager.Instance.PercentFilled;
        }

        void InstantiateIcons()
        {
            foreach (BountyState bounty in BountyManager.Instance.Bounties)
            {
                GameObject inst = Funcs.UI.Instantiate(BountyObjectSlot, bountySlotsParent, Vector3.zero);

                BountyObject obj = inst.GetComponent<BountyObject>();

                obj.SetBounty(bounty);
            }
        }


        // = = = Button Callbacks = = =
        public void OnClaimPoints()
        {
            BountyManager.Instance.ClaimPoints((code, data) => { });
        }
    }
}
