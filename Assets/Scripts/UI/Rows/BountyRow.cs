using System;

using UnityEngine;
using UnityEngine.UI;

using SimpleJSON;

using BountyID = BountyData.BountyID;

public class BountyRow : MonoBehaviour
{
    [SerializeField] BountyID bounty;

    [Header("Components")]
    [SerializeField] Button BountyButton;
    [SerializeField] Text BountyButtonText;
    [Space]
    [SerializeField] Slider ProgressBar;
    [Space]
    [SerializeField] Text nameText;
    [SerializeField] Text rewardText;
    [SerializeField] Text durationText;

    // Flag to stop double requests mapping
    bool isSendingRequest = false;

    void Awake()
    {
        var data = StaticData.Bounties.Get(bounty);

        nameText.text       = data.name;
        rewardText.text     = data.bountyReward.ToString() + "x Bounty Points";
        durationText.text   = data.duration.ToString() + "s";
    }

    void FixedUpdate()
    {
        bool isBountyOngoing = GameState.Bounties.TryGetBounty(bounty, out BountyState state);

        // Set soem defautl values ready to be updated
        ProgressBar.value = 0.0f; 
        BountyButtonText.text = "Start";
        BountyButton.interactable = !isSendingRequest;

        if (isBountyOngoing)
        {
            float progressPercent = GetCompleteProgress();

            BountyButtonText.text = progressPercent >= 1.0f ? "Collect" : "Processing";

            BountyButton.interactable = !isSendingRequest && progressPercent >= 1.0f;

            ProgressBar.value = progressPercent;
        }
    }

    // === Button Callbacks ===
    public void OnButtonDown()
    {
        JSONNode node = Utils.Json.GetDeviceNode();

        // Bounty is ongoing, so we need to check the status and hopefully collect the rewards
        if (GameState.Bounties.TryGetBounty(bounty, out BountyState state))
        {
            if (GetCompleteProgress() >= 1.0f)
            {
                node.Add("claimBountyId", (int)bounty);

                Server.ClaimBounty(this, OnClaimBountyCallback, node);
            }
        }

        // User wants to start this bounty, check if the user can
        else
        {
            node.Add("startBountyId", (int)bounty);

            Server.StartBounty(this, OnStartBountyCallback, node);
        }

        isSendingRequest = true;
    }

    // === Server Callbacks ===

    void OnStartBountyCallback(long code, string compressed)
    {
        isSendingRequest = false;

        if (code == 200)
        {
            JSONNode node = Utils.Json.Decode(compressed);

            GameState.Bounties.Set(bounty, node["startTime"].AsLong);
        }
    }

    void OnClaimBountyCallback(long code, string compressed)
    {
        isSendingRequest = false;

        if (code == 200)
        {
            JSONNode node = Utils.Json.Decode(compressed);

            GameState.Player.Update(node);

            GameState.Bounties.Remove(bounty);
        }
    }

    // === Helper Methods ===
    float GetCompleteProgress()
    {
        if (GameState.Bounties.TryGetBounty(bounty, out BountyState state))
        {
            var staticBountyData = StaticData.Bounties.Get(bounty);

            return (float)((DateTime.UtcNow - state.startTime).TotalSeconds) / staticBountyData.duration;
        }

        return 0.0f;
    }
}