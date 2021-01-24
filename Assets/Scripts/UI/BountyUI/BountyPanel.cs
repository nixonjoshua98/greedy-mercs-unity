using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs.UI.Bounties
{
    public class BountyPanel : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] Text bountyPointsText;
        [SerializeField] Text bountyIncomeText;
        [SerializeField] Text claimButtonText;

        [Space]
        [SerializeField] Slider collectionSlider;

        [Space]
        [SerializeField] Button claimButton;

        [Header("Prefabs")]
        [SerializeField] GameObject BountyIconObject;

        [Header("References")]
        [SerializeField] Transform bountyIconsParent;

        List<GameObject> icons;

        int numBountiesDisplayed = 0;

        bool claimingBounties;

        void Awake()
        {
            icons = new List<GameObject>();

            claimingBounties = false;
        }

        void OnEnable()
        {
            CheckNewBounty();

            InvokeRepeating("CheckNewBounty", 1.0f, 1.0f);
        }

        void OnDisable()
        {
            CancelInvoke("CheckNewBounty");
        }

        void CheckNewBounty()
        {
            if (GameState.Bounties.Unlocked().Count != numBountiesDisplayed)
            {
                foreach (GameObject o in icons)
                    Destroy(o);

                CreateIcons();
            }
        }

        void CreateIcons()
        {
            numBountiesDisplayed = 0;

            icons = new List<GameObject>();

            foreach (var bounty in GameState.Bounties.Unlocked())
            {
                numBountiesDisplayed++;

                GameObject inst = Utils.UI.Instantiate(BountyIconObject, bountyIconsParent, Vector3.one);

                BountyIcon bountyIcon = inst.GetComponent<BountyIcon>();

                bountyIcon.SetBounty(bounty.Value);

                icons.Add(inst);
            }
        }

        void FixedUpdate()
        {
            bountyPointsText.text = Utils.Format.FormatNumber(GameState.Player.bountyPoints);

            claimButton.interactable = !claimingBounties && GameState.Bounties.CurrentClaimAmount > 0;

            UpdateSlider();
        }

        void UpdateSlider()
        {
            collectionSlider.value = GameState.Bounties.PercentFilled;

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
            node.Add("lastClaimTime", GameState.Bounties.lastClaimTime.ToUnixMilliseconds());

            claimingBounties = true;

            Server.ClaimBounty(this, OnBountyClaim, node);
        }

        // === Server Callbacks ===
        void OnBountyClaim(long code, string compressed)
        {
            claimingBounties = false;

            if (code == 200)
            {
                var node = Utils.Json.Decompress(compressed);

                GameState.Player.bountyPoints += node["earnedBountyPoints"].AsLong;

                GameState.Bounties.lastClaimTime = DateTimeOffset.FromUnixTimeMilliseconds(node["lastClaimTime"].AsLong).DateTime;
            }
        }
    }
}