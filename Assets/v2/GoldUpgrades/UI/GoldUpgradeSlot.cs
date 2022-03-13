using TMPro;
using UnityEngine;

namespace GM.GoldUpgrades.UI
{
    public class GoldUpgradeSlot : GM.Core.GMMonoBehaviour
    {
        [Header("Components")]
        public TMP_Text LevelText;
        public TMP_Text DamageText;

        GoldUpgradeState Upgrade => App.GoldUpgrades.TapUpgrade;

        void FixedUpdate()
        {
            LevelText.text = $"Lvl. <color=orange>{Upgrade.Level}</color>";
            DamageText.text = $"<color=orange>{Format.Number(App.GMCache.TapUpgradeDamage)}</color> DMG";
        }

        // = Callbacks = //

        public void Button_Upgrade()
        {
            BigDouble upgradeCost = App.GMCache.TapUpgradeCost(1);

            bool canAffordUpgrade = App.Inventory.Gold >= upgradeCost;

            if (canAffordUpgrade)
            {
                Upgrade.Level += 1;

                App.Inventory.Gold -= upgradeCost;
            }
        }
    }
}
