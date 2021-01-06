
using UnityEngine;
using UnityEngine.UI;

using HeroPassiveUnlock = CharacterData.HeroPassiveUnlock;
using PassiveSkill      = PassivesData.PassiveSkill;

public class CharacterPassiveRow : MonoBehaviour
{
    public Text UnlockText;
    public Text SkillNameText;
    public Text DescriptionText;

    public Image PanelImage;

    public void UpdatePanel(UpgradeState heroState, HeroPassiveUnlock unlock, PassiveSkill skill)
    {
        // Name
        SkillNameText.text = skill.name;

        // Unlock Level
        UnlockText.text = UnlockText.text.Replace("{level}", unlock.unlockLevel.ToString());

        string desc = skill.value < 1 ? "Increase your {type} by {value}" : "Multiply your {type} by {value}";

        // Description
        DescriptionText.text = desc
            .Replace("{value}", "<color=orange>" + (skill.value * 100).ToString() + "%</color>")
            .Replace("{type}",  "<color=orange>" + Utils.Generic.BonusToString(skill.bonusType) + "</color>");


        if (heroState.level < unlock.unlockLevel)
        {
            UnlockText.color        = MultiplyColorAlpha(UnlockText.color, 0.25f);
            SkillNameText.color     = MultiplyColorAlpha(SkillNameText.color, 0.25f);
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
