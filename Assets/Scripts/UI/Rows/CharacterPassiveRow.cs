
using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs
{
    public class CharacterPassiveRow : MonoBehaviour
    {
        public Text UnlockText;
        public Text DescriptionText;

        public Image PanelImage;

        public void UpdatePanel(UpgradeState heroState, CharacterPassive passive)
        {
            UnlockText.text = passive.unlockLevel.ToString();

            DescriptionText.text = (passive.value * 100).ToString() + "% " + Utils.Generic.BonusToString(passive.bonusType);

            if (heroState.level < passive.unlockLevel)
            {
                UnlockText.color        = MultiplyColorAlpha(UnlockText.color, 0.25f);
                DescriptionText.color   = MultiplyColorAlpha(DescriptionText.color, 0.25f);
                PanelImage.color        = MultiplyColorAlpha(PanelImage.color, 0.25f);
            }
        }

        static Color MultiplyColorAlpha(Color color, float val)
        {
            color.a *= val;

            return color;
        }
    }
}