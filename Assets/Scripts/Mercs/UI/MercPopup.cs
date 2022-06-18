using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GM.Mercs.Data;
using System;

namespace GM.Mercs.UI
{
    public class MercPopup : GM.Core.GMMonoBehaviour
    {
        private GM.Common.Enums.MercID _mercID;

        [Header("Text Elements")]
        [SerializeField] TMP_Text NameText;
        [SerializeField] TMP_Text LevelText;
        [SerializeField] TMP_Text RechargeText;
        [SerializeField] TMP_Text BaseDamageText;

        [SerializeField] Image IconImage;

        AggregatedMercData Merc { get => App.Mercs.GetMerc(_mercID); }

        public void Initialize(AggregatedMercData merc)
        {
            _mercID = merc.ID;

            UpdateUI();
        }

        private void UpdateUI()
        {
            NameText.text = Merc.Name;
            RechargeText.text = $"{Math.Round(Merc.RechargeRate, 1)}s";
            BaseDamageText.text = Mathf.FloorToInt(Merc.BaseDamage).ToString();
            LevelText.text = $"Lv <color=orange>{Merc.CurrentLevel}</color>";

            IconImage.sprite = Merc.Icon;
        }

        /* Event Listeners */

        public void OnCloseButton()
        {
            Destroy(gameObject);
        }
    }
}