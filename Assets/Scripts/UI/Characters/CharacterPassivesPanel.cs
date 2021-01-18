using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace UI.Characters
{
    using Data.Characters;

    public class CharacterPassivesPanel : MonoBehaviour
    {
        [SerializeField] GameObject SkillRow;
        [Space]
        [SerializeField] GameObject ScrollContent;


        public void SetHero(CharacterID chara)
        {
            StartCoroutine(Create(chara));
        }

        IEnumerator Create(CharacterID chara)
        {
            List<CharacterPassive> passives = StaticData.CharacterList.Get(chara).passives;

            UpgradeState heroState = GameState.Characters.Get(chara);

            foreach (CharacterPassive passive in passives)
            {
                GameObject skillRow = Instantiate(SkillRow, ScrollContent.transform);

                skillRow.GetComponent<CharacterPassiveRow>().UpdatePanel(heroState, passive);

                yield return new WaitForFixedUpdate();
            }
        }
    }
}