
using System;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


namespace GM.BountyShop
{

    public class BountyShopUIController : MonoBehaviour
    {
        [Header("Components - UI")]
        [SerializeField] Text shopRefreshText;

        public UnityEvent OnShopRefreshed;

        void OnEnable()
        {
            BountyShopManager.Instance.Refresh(() => { OnShopRefreshed.Invoke(); });
        }

        void FixedUpdate()
        {
            TimeSpan timeUntilReset = Funcs.TimeUntil(GreedyMercs.StaticData.NextDailyReset);

            shopRefreshText.text = string.Format("Refreshes in {0}", Funcs.Format.Seconds(timeUntilReset.TotalSeconds));
        }
    }
}