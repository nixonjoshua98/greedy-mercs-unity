using GM.Common;
using GM.Mercs.Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Mercs.UI
{
    public class MercPopup : GM.UI.PopupBase
    {
        private GM.Enums.MercID _mercID;

        [Header("Prefabs")]
        [SerializeField] GameObject PassiveSlotObject;

        [Header("Text Elements")]
        [SerializeField] TMP_Text NameText;
        [SerializeField] TMP_Text LevelText;
        [SerializeField] TMP_Text RechargeText;
        [SerializeField] TMP_Text DamageText;

        [Header("Transforms")]
        [SerializeField] Transform PassivesParent;

        [Space]

        [SerializeField] GM.UI.GenericGradeSlot GradeSlot;

        AggregatedMercData Merc { get => App.Mercs.GetMerc(_mercID); }

        public void Initialize(AggregatedMercData merc)
        {
            _mercID = merc.ID;

            InstantiatePassiveSlots();

            UpdateUI();

            ShowInnerPanel();
        }

        private void UpdateUI()
        {
            NameText.text           = Merc.Name;
            DamageText.text         = $"{Format.Number(Merc.DamagePerAttack)}";
            RechargeText.text       = $"{Math.Round(Merc.RechargeRate, 2)}s";
            LevelText.text          = $"{Merc.CurrentLevel}";

            GradeSlot.Intialize(Merc);
        }

        void InstantiatePassiveSlots()
        {
            Merc.Passives.ForEach(passive =>
            {
                var slot = this.Instantiate<MercPassiveSlot>(PassiveSlotObject, PassivesParent);

                slot.Initialize(passive);
            });
        }

        /* Event Listeners */

        public void OnCloseButton()
        {
            Destroy(gameObject);
        }
    }
}