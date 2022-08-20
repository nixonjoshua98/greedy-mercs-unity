using SRC.Common;
using SRC.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SRC.Artefacts.UI
{
    public class ArtefactSlot : ArtefactUIObject
    {
        [Header("Prefabs")]
        public GameObject ArtefactPopupObject;

        [Header("Text References")]
        [SerializeField] private TMP_Text QuantityText;
        [SerializeField] private TMP_Text UpgradeCostText;
        [SerializeField] private TMP_Text NameText;
        [SerializeField] private TMP_Text LevelText;
        [SerializeField] private TMP_Text BonusText;
        [Space]
        [SerializeField] private Button UpgradeButton;

        [Header("...")]
        [SerializeField] private GenericGradeItem GradeSlot;

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

            QuantityText.text = Artefact.IsMaxLevel ? "Max Level" : $"x{BuyAmount}";
            UpgradeCostText.text = Artefact.IsMaxLevel ? "" : Format.Number(ugradeCost);

            LevelText.text = $"Lv <color=orange>{Artefact.CurrentLevel}</color>";
            BonusText.text = $" <color=orange>{Format.Number(Artefact.BonusValue, Artefact.BonusType)}</color> {Format.BonusType(Artefact.BonusType)}";

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