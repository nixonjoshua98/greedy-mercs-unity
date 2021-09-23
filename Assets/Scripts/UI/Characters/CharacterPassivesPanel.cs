using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace GM
{
    using GM.Data;

    using GM.Units;

    public class CharacterPassivesPanel : Core.GMMonoBehaviour
    {
        [SerializeField] GameObject SkillRow;
        [Space]
        [SerializeField] GameObject ScrollContent;


        public void SetHero(MercID chara)
        {
            StartCoroutine(Create(chara));
        }

        IEnumerator Create(MercID merc)
        {
            GM.Mercs.Data.MercGameData data = App.Data.Mercs.GetMerc(merc).GameValues;
            MercState state = MercenaryManager.Instance.GetState(merc);

            foreach (GM.Mercs.Data.MercPassiveSkillData passive in data.Passives)
            {
                GameObject skillRow = Instantiate(SkillRow, ScrollContent.transform);

                skillRow.GetComponent<CharacterPassiveRow>().UpdatePanel(state, passive);

                yield return new WaitForFixedUpdate();
            }
        }
    }
}