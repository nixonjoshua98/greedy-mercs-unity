using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace UI.Characters
{
    using CharacterData;
    using PassivesData;

    public class CharacterPassivesPanel : MonoBehaviour
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
            List<HeroPassiveUnlock> unlocks = StaticData.Characters.GetPassives(showingHero);

            UpgradeState heroState = GameState.Characters.Get(showingHero);

            foreach (HeroPassiveUnlock unlock in unlocks)
            {
                PassiveSkill skill = StaticData.Passives.Get(unlock.skill);

                GameObject skillRow = Instantiate(SkillRow, ScrollContent.transform);

                skillRow.GetComponent<CharacterPassiveRow>().UpdatePanel(heroState, unlock, skill);

                yield return new WaitForFixedUpdate();
            }
        }
    }
}