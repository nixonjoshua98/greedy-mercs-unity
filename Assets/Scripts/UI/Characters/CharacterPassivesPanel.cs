using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace GM
{
    using GM.Data;

    using GM.Units;

    public class CharacterPassivesPanel : MonoBehaviour
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
            MercData data = GameData.Get.Mercs.Get(merc);
            MercState state = MercenaryManager.Instance.GetState(merc);

            foreach (MercPassiveData passive in data.Passives)
            {
                GameObject skillRow = Instantiate(SkillRow, ScrollContent.transform);

                skillRow.GetComponent<CharacterPassiveRow>().UpdatePanel(state, passive);

                yield return new WaitForFixedUpdate();
            }
        }
    }
}