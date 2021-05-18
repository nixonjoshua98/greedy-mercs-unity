using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM.Bounty
{
    using GameState = GreedyMercs.GameState;

    public class BountyTab : MonoBehaviour
    {
        [SerializeField] Text bountyPointsText;

        void FixedUpdate()
        {
            bountyPointsText.text = GameState.Inventory.bountyPoints.ToString();
        }
    }
}
