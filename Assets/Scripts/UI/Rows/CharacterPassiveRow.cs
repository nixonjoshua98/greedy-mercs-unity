
using UnityEngine;
using UnityEngine.UI;

using HeroPassiveUnlock = CharacterData.HeroPassiveUnlock;
using PassiveSkill      = PassivesData.PassiveSkill;

public class CharacterPassiveRow : MonoBehaviour
{
    public Text UnlockText;
    public Text DescriptionText;

    public Image PanelImage;

    public void UpdatePanel(UpgradeState heroState, HeroPassiveUnlock unlock, PassiveSkill skill)
    {
        UnlockText.text = unlock.unlockLevel.ToString();

        // Description
        DescriptionText.text = (skill.value * 100).ToString() + "% " + Utils.Generic.BonusToString(skill.bonusType);

        if (heroState.level < unlock.unlockLevel)
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
