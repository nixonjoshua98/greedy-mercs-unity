using System.Collections;
using System.Collections.Generic;

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
            shopRefreshText.text = "Refreshes in ???";
        }
    }
}