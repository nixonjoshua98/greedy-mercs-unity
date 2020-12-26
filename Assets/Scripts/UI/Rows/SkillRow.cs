
using UnityEngine;
using UnityEngine.UI;

public class SkillRow : MonoBehaviour
{
    public Text UnlockText;
    public Text SkillNameText;
    public Text DescriptionText;

    public Image PanelImage;

    public void UpdatePanel(HeroState heroState, HeroPassiveUnlock unlock, HeroPassiveSkill skill)
    {
        // Name
        SkillNameText.text = skill.name;

        // Unlock Level
        UnlockText.text = UnlockText.text.Replace("{level}", unlock.unlockLevel.ToString());

        // Description
        DescriptionText.text = DescriptionText.text
            .Replace("{skillValue}", skill.value.ToString())
            .Replace("{skillTypeText}", HeroResources.PassiveTypeToString(skill.bonusType));


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
