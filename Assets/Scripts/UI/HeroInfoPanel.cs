using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroInfoPanel : MonoBehaviour
{
    HeroID showingHero;

    [SerializeField] GameObject SkillRow;
    [Space]
    [SerializeField] GameObject ScrollContent;


    public void SetHero(HeroID hero)
    {
        showingHero = hero;
    }

    IEnumerator Start()
    {
        List<HeroPassiveUnlock> unlocks = ServerData.GetHeroPassiveSkills(showingHero);

        foreach (var unlock in unlocks)
        {
            var skill = ServerData.GetPassiveData(unlock.skill);

            GameObject skillRow = Instantiate(SkillRow, ScrollContent.transform);

            SkillRow skillRowScript = skillRow.GetComponent<SkillRow>();

            // Name
            skillRowScript.SkillNameText.text = skill.name;

            // Unlock Level
            skillRowScript.UnlockText.text = skillRowScript.UnlockText.text.Replace("{level}", unlock.unlockLevel.ToString());

            // Description
            skillRowScript.DescriptionText.text = skillRowScript.DescriptionText.text
                .Replace("{skillValue}", skill.value.ToString())
                .Replace("{skillTypeText}", HeroResources.PassiveTypeToString(skill.type));

            yield return new WaitForFixedUpdate();
        }
    }

    public void OnClose()
    {
        Destroy(gameObject);
    }
}
