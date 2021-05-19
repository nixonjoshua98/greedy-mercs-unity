using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM.BountyShop
{
    using Utils = GreedyMercs.Utils;
    using GameState = GreedyMercs.GameState;

    public class BountyShopSection : MonoBehaviour
    {
        [SerializeField] Text shopRefreshText;

        void FixedUpdate()
        {
            shopRefreshText.text = string.Format("Refreshes in {0}", Utils.Format.FormatSeconds(GameState.TimeUntilNextReset));
        }
    }
}