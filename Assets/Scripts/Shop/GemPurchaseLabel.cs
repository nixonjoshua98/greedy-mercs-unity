using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using Coffee.UIEffects;

namespace GreedyMercs.Shop.UI
{
    public class GemPurchaseLabel : MonoBehaviour
    {
        [Header("Purchase Components")]
        [SerializeField] Text purchaseText;
        [SerializeField] Button purchaseButton;

        [Header("Overlay Components")]
        [SerializeField] GameObject overlayObject;
        [SerializeField] Text overlayText;

        [Header("Effects")]
        [SerializeField] UIShiny shinyEffect;

        public void ToggleOverlay(bool val) => overlayObject.SetActive(val);
        public void SetOverlayText(string txt) => overlayText.text = txt;

        public void ToggleActive(bool val)
        {
            purchaseButton.interactable = val;

            if (val)
                shinyEffect.Play(reset: false);
            else
                shinyEffect.Stop(reset: true);
        }
    }
}