using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GreedyMercs.Shop.UI
{
    public class GemPurchaseLabel : MonoBehaviour
    {
        [SerializeField] Text purchaseText;
        [SerializeField] Button purchaseButton;

        [Header("Overlay Components")]
        [SerializeField] Text overlayText;
        [SerializeField] GameObject overlayObject;

        public void SetButtonEvent(UnityAction e)
        {
            purchaseButton.onClick.RemoveAllListeners();

            purchaseButton.onClick.AddListener(e);
        }

        public void ToggleOverlay(bool val) => overlayObject.SetActive(val);
        public void SetOverlayText(string txt) => overlayText.text = txt;
    }
}