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

        [Space]
        [SerializeField] Button refreshButton;

        bool isRefreshing;

        void Awake()
        {
            isRefreshing = false;

            EnableShopUI();
        }

        void OnEnable()
        {
            InvokeRepeating("UpdateResetTime", 0.0f, 0.5f);
        }

        void OnDisable()
        {
            CancelInvoke("UpdateResetTime");
        }

        void FixedUpdate()
        {
            bountyPointsText.text = Utils.Format.FormatNumber(GameState.Player.bountyPoints);
        }

        void UpdateResetTime()
        {
            if (!Formulas.Server.BountyShopNeedsRefresh)
            {
                dailyResetText.text = Utils.Format.FormatSeconds(Formulas.Server.SecondsUntilDailyReset);
            }

            else
            {
                CancelInvoke("UpdateResetTime");

                EnableRefreshUI();
            }
        }

        void EnableRefreshUI()
        {
            refreshButton.enabled = true;

            dailyResetText.text = "Refresh";
        }

        void EnableShopUI()
        {
            refreshButton.enabled = false;
        }

        public void RefreshBountyShop()
        {
            if (Formulas.Server.BountyShopNeedsRefresh && !isRefreshing)
            {
                Server.RefreshBountyShop(this, ServerCallback, Utils.Json.GetDeviceNode());
            }
        }

        void ServerCallback(long code, string compressed)
        {
            if (code == 200)
            {
                EnableShopUI();

                isRefreshing = false;

                JSONNode node = Utils.Json.Decompress(compressed);

                GameState.BountyShop.Update(node);

                InvokeRepeating("UpdateResetTime", 0.0f, 0.5f);
            }
        }
    }
}