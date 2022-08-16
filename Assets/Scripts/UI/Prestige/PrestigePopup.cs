using System;
using TMPro;
using UnityEngine;

namespace SRC.UI
{
    public class PrestigePopup : SRC.UI.PopupBase
    {
        bool _IsInitialized = false;

        [Header("Text Elements")]
        [SerializeField] TMP_Text PrestigePointsText;
        [SerializeField] TMP_Text BountiesDefeatedText;

        Action OnPrestigeAction;

        public void Initialize(Action action)
        {
            _IsInitialized = true;

            OnPrestigeAction = action;

            FixedUpdate(); // Early update

            ShowInnerPanel();
        }

        void FixedUpdate()
        {
            if (!_IsInitialized)
                return;

            UpdatePrestigePointsText();
            UpdateBountiesDefeatedText();
        }

        void UpdatePrestigePointsText()
        {
            var basePoints = App.Values.BasePrestigePoints();
            var bonusPoints = App.Values.BonusPrestigePoints();

            // BasePoints + BonusPoints
            string quantity = $"{Format.Number(basePoints)}{(bonusPoints > 0 ? $"+ {Format.Number(bonusPoints)}" : "")}";

            PrestigePointsText.text = $"<color=orange>{quantity}</color> Runestones";
        }

        void UpdateBountiesDefeatedText()
        {
            BountiesDefeatedText.text = $"Defeated <color=green>{App.Bounties.BountiesDefeatedSincePrestige}</color> Bounties";
        }

        /* Event Listeners */

        public void OnCancelPrestige()
        {
            Destroy(gameObject);
        }

        public void OnConfirmPrestige()
        {
            OnPrestigeAction.Invoke();
        }
    }
}
