using System;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;

namespace BountyUI
{
    public class BountyTab : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] Text bountyPoints;
        [SerializeField] Text bountyIncomeText;
        [SerializeField] Text claimButtonText;
        [SerializeField] Slider collectionSlider;

        [Header("Prefabs")]
        [SerializeField] GameObject bountyIconObject;

        [Header("References")]
        [SerializeField] Transform bountyIconsParent;

        IEnumerator Start()
        {
            foreach (var bounty in StaticData.Bounties.All())
            {
                GameObject inst = Utils.UI.Instantiate(bountyIconObject, bountyIconsParent, Vector3.one);

                BountyIcon bountyIcon = inst.GetComponent<BountyIcon>();

                bountyIcon.SetBountyIndex(bounty.Key);

                yield return new WaitForFixedUpdate();
            }
        }

        void FixedUpdate()
        {
            bountyPoints.text = Utils.Format.FormatNumber(GameState.Player.bountyPoints) + " Bounty Points";

            UpdateSlider();
        }

        void UpdateSlider()
        {
            collectionSlider.value  = GameState.Bounties.PercentFilled;

            claimButtonText.text    = string.Format("Collect ({0})", GameState.Bounties.CurrentClaimAmount);
            bountyIncomeText.text   = string.Format("{0} / hour (Max {1})", GameState.Bounties.HourlyIncome, GameState.Bounties.MaxClaimAmount);
        }


        // === Button Callbacks ===
        public void OnClaimBountyPoints()
        {
            if (GameState.Bounties.CurrentClaimAmount <= 0)
                return;

            var node = Utils.Json.GetDeviceNode();

            node.Add("currentStage", GameState.Stage.stage);

            Server.ClaimBounty(this, OnBountyClaim, node);

        }

        // === Server Callbacks ===
        void OnBountyClaim(long code, string compressed)
        {
            if (code == 200)
            {
                var node = Utils.Json.Decode(compressed);

                GameState.Player.Update(node);
                GameState.Bounties.Update(node);
            }
        }
    }
}