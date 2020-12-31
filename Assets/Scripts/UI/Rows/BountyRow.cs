
using UnityEngine;
using UnityEngine.UI;

using BountyID = BountyData.BountyID;

public class BountyRow : MonoBehaviour
{
    [SerializeField] BountyID bounty;

    [Header("Components")]
    [SerializeField] Text nameText;
    [SerializeField] Text rewardText;
    [SerializeField] Text durationText;

    void Awake()
    {
        var data = StaticData.Bounties.Get(bounty);

        nameText.text       = data.name;
        rewardText.text     = data.bountyReward.ToString() + "x Bounty Points";
        durationText.text   = data.duration.ToString() + "s";
    }
}