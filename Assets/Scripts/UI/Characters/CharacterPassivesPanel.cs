using GM.Units;
using System.Collections;
using UnityEngine;

namespace GM
{
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
            GM.Mercs.Data.FullMercData data = App.Data.Mercs[merc];

            foreach (GM.Mercs.Data.MercPassiveSkillData passive in data.GameData.Passives)
            {
                GameObject skillRow = Instantiate(SkillRow, ScrollContent.transform);

                skillRow.GetComponent<CharacterPassiveRow>().UpdatePanel(data, passive);

                yield return new WaitForFixedUpdate();
            }
        }
    }
}