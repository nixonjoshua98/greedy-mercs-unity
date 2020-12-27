
using UnityEngine;
using UnityEngine.UI;

public class SkillRow : MonoBehaviour
{
    public Text UnlockText;
    public Text SkillNameText;
    public Text DescriptionText;

    public Image PanelImage;

    public void UpdatePanel(UpgradeState heroState, HeroPassiveUnlock unlock, HeroPassiveSkill skill)
    {
        // Name
        SkillNameText.text = skill.name;

        // Unlock Level
        UnlockText.text = UnlockText.text.Replace("{level}", unlock.unlockLevel.ToString());

        // Description
        DescriptionText.text = (skill.description == "" ? DescriptionText.text : skill.description)
            .Replace("{skillValue}", skill.value.ToString())
            .Replace("{skillTypeText}", CharacterResources.PassiveTypeToString(skill.bonusType));


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
