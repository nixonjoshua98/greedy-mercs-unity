using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace GreedyMercs
{
    using GM.Characters;

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
            MercData data = StaticData.Mercs.GetMerc(chara);

            UpgradeState heroState = GameState.Characters.Get(chara);

            foreach (MercPassiveData passive in data.Passives)
            {
                GameObject skillRow = Instantiate(SkillRow, ScrollContent.transform);

                skillRow.GetComponent<CharacterPassiveRow>().UpdatePanel(heroState, passive);

                yield return new WaitForFixedUpdate();
            }
        }
    }
}