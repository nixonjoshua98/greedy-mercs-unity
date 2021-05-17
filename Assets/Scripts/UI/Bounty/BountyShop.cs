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

        void UpdateUI()
        {
            if (GameState.BountyShop.IsValid)
            {
                string temp = Utils.Format.FormatSeconds(GameState.TimeUntilNextReset);

                dailyResetText.text = string.Format("Shop refresh in {0}", temp);
            }

            else
            {
                dailyResetText.text = "...";

                if (!isRefreshing)
                {
                    isRefreshing = true;

                    Server.RefreshBountyShop(ServerCallback, Utils.Json.GetDeviceInfo());
                }
            }
        }

        void ServerCallback(long code, string compressed)
        {
            isRefreshing = false;

            if (code == 200)
            {
                JSONNode node = Utils.Json.Decompress(compressed);

                GameState.BountyShop.Update(node);
            }
        }
    }
}