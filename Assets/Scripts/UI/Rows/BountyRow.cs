
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

    [Header("Prefabs")]
    [SerializeField] GameObject ErroMessageObject;

    void Awake()
    {
        var data = StaticData.Bounties.Get(bounty);

        nameText.text       = data.name;
        rewardText.text     = data.bountyReward.ToString() + "x Bounty Points";
        durationText.text   = data.duration.ToString() + "s";
    }

    void OnEnable()
    {
        UpdateBounty();

        InvokeRepeating("UpdateBounty", 0.5f, 0.5f);
    }

    void OnDisable()
    {
        CancelInvoke("UpdateBounty");
    }

    void UpdateBounty()
    {
        bool isBountyOngoing = GameState.Bounties.TryGetBounty(bounty, out BountyState state);

        BountyButtonText.text = isBountyOngoing ? "Collect" : "Start";

        BountyButton.interactable = !isBountyOngoing;

        if (isBountyOngoing)
        {
            float progressPercent = GetCompleteProgress();

            if (progressPercent >= 1.0f)
            {
                BountyButtonText.text = "Collect";

                BountyButton.interactable = true;
            }

            ProgressBar.value = progressPercent;
        }

        else
        {
            ProgressBar.value = 0.0f;
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
    }

    // === Server Callbacks ===

    void OnStartBountyCallback(long code, string compressed)
    {
        if (code == 200)
        {
            JSONNode node = Utils.Json.Decode(compressed);

            GameState.Bounties.Set(bounty, node["startTime"].AsLong);
        }

        else
        {
            Utils.UI.ShowError(ErroMessageObject, "Start Bounty", "Failed to start a new bounty.");
        }

        UpdateBounty();
    }

    void OnClaimBountyCallback(long code, string compressed)
    {
        if (code == 200)
        {
            JSONNode node = Utils.Json.Decode(compressed);

            GameState.Player.Update(node);

            GameState.Bounties.Remove(bounty);
        }

        else
        {
            Utils.UI.ShowError(ErroMessageObject, "Start Bounty", "Failed to claim bounty reward.");
        }
    }

    // === Helper Methods ===
    float GetCompleteProgress()
    {
        if (GameState.Bounties.TryGetBounty(bounty, out BountyState state))
        {
            var staticBountyData = StaticData.Bounties.Get(bounty);

            double secondsSinceStart = (System.DateTime.UtcNow - state.startTime).TotalSeconds;

            return (float)(secondsSinceStart / staticBountyData.duration);
        }

        return 0.0f;
    }
}