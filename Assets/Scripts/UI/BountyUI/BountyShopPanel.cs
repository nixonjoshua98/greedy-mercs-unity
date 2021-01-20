using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs.UI.Bounties
{
    public class BountyShopPanel : MonoBehaviour
    {
        [SerializeField] Text dailyResetText;

        void Awake()
        {
            InvokeRepeating("UpdateResetTime", 0.0f, 0.5f);
        }

        void UpdateResetTime()
        {
            dailyResetText.text = Utils.Format.FormatSeconds(Formulas.Server.SecondsUntilDailyReset);
        }

        
        // === Button Callbacks ===
        public void RefreshBountyShop()
        {
            Server.RefreshBountyShop(this, OnServerCallback, Utils.Json.GetDeviceNode());
        }

        // === Server Callbacks ===
        void OnServerCallback(long code, string compressed)
        {

        }
    }
}