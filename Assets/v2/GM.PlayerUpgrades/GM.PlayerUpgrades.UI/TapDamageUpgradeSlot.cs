using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BonusType = GM.Common.Enums.BonusType;

namespace GM.PlayerUpgrades.UI
{
    public class TapDamageUpgradeSlot : Core.GMMonoBehaviour
    {
        int level = 0;
        BigDouble damage => level * 4.76;

        [Header("Components")]
        public TMP_Text LevelText;
        public TMP_Text DamageText;

        void FixedUpdate()
        {
            LevelText.text = $"Level {level}";
            DamageText.text = $"<color=orange>{Format.Number(damage)}</color> {Format.Bonus(BonusType.FLAT_TAP_DAMAGE)}";
        }

        public void OnUpgradeButton()
        {
            level++;
        }
    }
}
