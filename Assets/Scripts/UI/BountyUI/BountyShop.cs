using SimpleJSON;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs.UI.Bounties
{
    public class BountyShop : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] Text dailyResetText;
        [SerializeField] Text bountyPointsText;

        bool isRefreshing = false;

        void OnEnable()
        {
            isRefreshing = false;

            InvokeRepeating("UpdateUI", 0.0f, 0.5f);
        }

        void OnDisable()
        {
            CancelInvoke("UpdateUI");
        }

        void FixedUpdate()
        {
            bountyPointsText.text = Utils.Format.FormatNumber(GameState.Player.bountyPoints);
        }

        void UpdateUI()
        {
            if (GameState.BountyShop.IsValid)
            {
                dailyResetText.text = Utils.Format.FormatSeconds(GameState.TimeUntilNextReset);
            }

            else
            {
                dailyResetText.text = "...";

                if (!isRefreshing)
                {
                    isRefreshing = true;

                    Server.RefreshBountyShop(this, ServerCallback, Utils.Json.GetDeviceNode());
                }
            }
        }

        void ServerCallback(long code, string compressed)
        {
            isRefreshing = false;

            if (code == 200)
            {
                GameState.BountyShop.Update(Utils.Json.Decompress(compressed));
            }
        }
    }
}