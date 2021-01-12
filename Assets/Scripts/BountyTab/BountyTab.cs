using System;
using System.Collections;
using System.Collections.Generic;

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
        [SerializeField] GameObject BountyIconObject;

        [Header("References")]
        [SerializeField] Transform bountyIconsParent;

        List<GameObject> icons;

        int numRelicsDisplayed = 0;

        void Awake()
        {
            icons = new List<GameObject>();
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
            if (GameState.Bounties.Unlocked().Count != numRelicsDisplayed)
            {
                foreach (GameObject o in icons)
                    Destroy(o);

                CreateIcons();
            }
        }    

        void CreateIcons()
        {
            numRelicsDisplayed = 0;

            icons = new List<GameObject>();

            foreach (var bounty in GameState.Bounties.Unlocked())
            {
                numRelicsDisplayed++;

                GameObject inst = Utils.UI.Instantiate(BountyIconObject, bountyIconsParent, Vector3.one);

                BountyIcon bountyIcon = inst.GetComponent<BountyIcon>();

                bountyIcon.SetBounty(bounty.Value);

                icons.Add(inst);
            }
        }

        void FixedUpdate()
        {
            bountyPoints.text = Utils.Format.FormatNumber(GameState.Player.bountyPoints);

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
            node.Add("lastClaimTime", GameState.Bounties.LastClaimTime.ToUnixMilliseconds());

            Server.ClaimBounty(this, OnBountyClaim, node);
        }

        // === Server Callbacks ===
        void OnBountyClaim(long code, string compressed)
        {
            if (code == 200)
            {
                var node = Utils.Json.Decompress(compressed);

                GameState.Player.Update(node);
                GameState.Bounties.Update(node);
            }
        }
    }
}