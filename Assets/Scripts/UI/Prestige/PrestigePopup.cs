using System;
using TMPro;
using UnityEngine;

namespace SRC.UI
{
    public class PrestigePopup : SRC.UI.PopupBase
    {
        private bool _IsInitialized = false;

        [Header("Text Elements")]
        [SerializeField] private TMP_Text PrestigePointsText;
        [SerializeField] private TMP_Text BountiesDefeatedText;
        private Action OnPrestigeAction;

        public void Initialize(Action action)
        {
            _IsInitialized = true;

            OnPrestigeAction = action;

            FixedUpdate(); // Early update

            ShowInnerPanel();
        }

        private void FixedUpdate()
        {
            if (!_IsInitialized)
                return;

            UpdatePrestigePointsText();
            UpdateBountiesDefeatedText();
        }

        private void UpdatePrestigePointsText()
        {
            var basePoints  = App.Bonuses.BonusPrestigePointsAtStage(App.GameState.Stage);
            var bonusPoints = App.Bonuses.PrestigePointsAtStage(App.GameState.Stage);

            // BasePoints + BonusPoints
            string quantity = $"{Format.Number(basePoints)}{(bonusPoints > 0 ? $"+ {Format.Number(bonusPoints)}" : "")}";

            PrestigePointsText.text = $"<color=orange>{quantity}</color> Runestones";
        }

        private void UpdateBountiesDefeatedText()
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
