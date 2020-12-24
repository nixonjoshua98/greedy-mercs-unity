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
        List<HeroPassiveUnlock> unlocks = StaticData.GetHeroPassiveSkills(showingHero);

        HeroState heroState = GameState.GetHeroState(showingHero);

        foreach (HeroPassiveUnlock unlock in unlocks)
        {
            HeroPassiveSkill skill = StaticData.GetPassiveData(unlock.skill);

            GameObject skillRow = Instantiate(SkillRow, ScrollContent.transform);

            skillRow.GetComponent<SkillRow>().UpdatePanel(heroState, unlock, skill);

            yield return new WaitForFixedUpdate();
        }
    }

    public void OnClose()
    {
        Destroy(gameObject);
    }
}
