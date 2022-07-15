using GM.Common;
using GM.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Artefacts.UI
{
    public class ArtefactSlot : ArtefactUIObject
    {
        [Header("Prefabs")]
        public GameObject ArtefactPopupObject;

        [Header("Text References")]
        [SerializeField] TMP_Text QuantityText;
        [SerializeField] TMP_Text UpgradeCostText;
        [SerializeField] TMP_Text NameText;
        [SerializeField] TMP_Text LevelText;
        [SerializeField] TMP_Text BonusText;
        [Space]
        [SerializeField] Button UpgradeButton;

        [Header("...")]
        [SerializeField] GenericGradeSlot GradeSlot;

        private Action<int, int> UpgradeCallback;

        private int _BuyAmount; // Raw value. We should use BuyAmount for most cases

        private int BuyAmount => MathsUtlity.NextMultipleMax(Artefact.CurrentLevel, _BuyAmount, Artefact.MaxLevel);

        public void Intialize(int artefactId, IntegerSelector selector, Action<int, int> callback)
        {
            UpgradeCallback = callback;

            selector.E_OnChange.AddListener(val =>
            {
                _BuyAmount = val;

                UpdateUI();
            });

            // Assign a default buy amount
            _BuyAmount = selector.CurrentValue;

            Intialize(artefactId);
        }

        protected override void OnIntialize()
        {
            GradeSlot.Intialize(Artefact);

            NameText.text = Artefact.Name;

            UpdateUI();
        }

        private void UpdateUI()
        {
            double ugradeCost = Artefact.UpgradeCost(BuyAmount);

            QuantityText.text       = Artefact.IsMaxLevel ? "Max Level" : $"x{BuyAmount}";
            UpgradeCostText.text    = Artefact.IsMaxLevel ? "" : Format.Number(ugradeCost);

            LevelText.text = $"Lv <color=orange>{Artefact.CurrentLevel}</color>";
            BonusText.text = $" <color=orange>{Format.Number(Artefact.Effect, Artefact.Bonus)}</color> {Format.BonusType(Artefact.Bonus)}";

            UpgradeButton.interactable = !Artefact.IsMaxLevel && ugradeCost <= App.Inventory.PrestigePoints;
        }

        /* Event Listeners */

        public void OnUpgradeButton()
        {
            UpgradeCallback.Invoke(Artefact.Id, BuyAmount);

            UpdateUI();
        }

        public void OnInfoButton()
        {
            this.InstantiateUI<ArtefactPanel>(ArtefactPopupObject).Intialize(Artefact.Id);
        }
    }
}