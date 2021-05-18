using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM.Bounty
{
    public class BountiesTab : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] GameObject BountyIconObject;

        [Header("Objects")]
        [SerializeField] Transform BountyParentTransform;

        [Header("Components")]
        [SerializeField] Text bountyIncomeText;
        [SerializeField] Text unclaimedTotalText;
        [Space]
        [SerializeField] Slider bountySlider;

        void Start()
        {
            InstantiateIcons();
        }

        void FixedUpdate()
        {
            bountyIncomeText.text = string.Format("{0} / hour (Max {1})", BountyManager.Instance.TotalHourlyIncome, BountyManager.Instance.TotalCapacity);

            bountySlider.value      = BountyManager.Instance.PercentFilled;
            unclaimedTotalText.text = string.Format("Collect ({0})", BountyManager.Instance.UnclaimedTotal);
        }

        void InstantiateIcons()
        {
            foreach (BountyState bounty in BountyManager.Instance.Bounties)
            {
                GameObject inst = Funcs.UI.Instantiate(BountyIconObject, BountyParentTransform, Vector3.zero);

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
