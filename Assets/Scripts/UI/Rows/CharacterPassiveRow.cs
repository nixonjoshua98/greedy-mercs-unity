
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Units
{
    public class CharacterPassiveRow : MonoBehaviour
    {
        public Text UnlockText;
        public Text DescriptionText;

        public Image PanelImage;

        public void UpdatePanel(GM.Mercs.Data.FullMercData state, GM.Mercs.Models.MercPassiveDataModel passive)
        {
            UnlockText.text = passive.UnlockLevel.ToString();

            DescriptionText.text = FormatString.Bonus(passive.Type, passive.Value);

            if (!state.UnlockedPassives.Contains(passive))
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