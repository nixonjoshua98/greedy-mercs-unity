﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CharacterSkillsPanel : MonoBehaviour
{
    CharacterID showingHero;

    [SerializeField] GameObject SkillRow;
    [Space]
    [SerializeField] GameObject ScrollContent;


    public void SetHero(CharacterID hero)
    {
        showingHero = hero;
    }

    IEnumerator Start()
    {
        List<HeroPassiveUnlock> unlocks = StaticData.GetCharPassives(showingHero);

        UpgradeState heroState = GameState.Characters.GetCharacter(showingHero);

        foreach (HeroPassiveUnlock unlock in unlocks)
        {
            HeroPassiveSkill skill = StaticData.GetPassive(unlock.skill);

            GameObject skillRow = Instantiate(SkillRow, ScrollContent.transform);

            skillRow.GetComponent<SkillRow>().UpdatePanel(heroState, unlock, skill);

            yield return new WaitForFixedUpdate();
        }
    }
}