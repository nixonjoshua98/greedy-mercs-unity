using GM.Mercs.Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Mercs.UI
{
    public class MercPopup : GM.UI.PopupBase
    {
        private GM.Common.Enums.MercID _mercID;

        [Header("Prefabs")]
        [SerializeField] GameObject PassiveSlotObject;

        [Header("Text Elements")]
        [SerializeField] TMP_Text NameText;
        [SerializeField] TMP_Text LevelText;
        [SerializeField] TMP_Text RechargeText;
        [Space]
        [SerializeField] Image IconImage;

        [Header("Transforms")]
        [SerializeField] Transform PassivesParent;

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
            RechargeText.text       = $"{Math.Round(Merc.RechargeRate, 1)}s";
            LevelText.text          = $"Lv <color=orange>{Merc.CurrentLevel}</color>";

            IconImage.sprite = Merc.Icon;
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