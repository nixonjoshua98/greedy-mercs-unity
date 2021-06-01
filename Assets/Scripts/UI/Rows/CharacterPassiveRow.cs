
using UnityEngine;
using UnityEngine.UI;

namespace GM.Characters
{
    public class CharacterPassiveRow : MonoBehaviour
    {
        public Text UnlockText;
        public Text DescriptionText;

        public Image PanelImage;

        public void UpdatePanel(UpgradeState heroState, MercPassiveData passive)
        {
            UnlockText.text = passive.UnlockLevel.ToString();

            DescriptionText.text = (passive.Value * 100).ToString() + "% " + GreedyMercs.Utils.Generic.BonusToString(passive.Type);

            if (heroState.level < passive.UnlockLevel)
            {
                UnlockText.color        = MultiplyColorAlpha(UnlockText.color, 0.5f);
                DescriptionText.color   = MultiplyColorAlpha(DescriptionText.color, 0.5f);
                PanelImage.color        = MultiplyColorAlpha(PanelImage.color, 0.5f);
            }
        }

        static Color MultiplyColorAlpha(Color color, float val)
        {
            color.a *= val;

            return color;
        }
    }
}