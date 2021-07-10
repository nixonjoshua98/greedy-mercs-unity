﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace GM
{
    using GM.Units;

    public class CharacterPassivesPanel : MonoBehaviour
    {
        [SerializeField] GameObject SkillRow;
        [Space]
        [SerializeField] GameObject ScrollContent;


        public void SetHero(UnitID chara)
        {
            StartCoroutine(Create(chara));
        }

        IEnumerator Create(UnitID chara)
        {
            MercData data   = StaticData.Mercs.GetMerc(chara);
            MercState state = MercenaryManager.Instance.GetState(chara);

            foreach (MercPassiveData passive in data.Passives)
            {
                GameObject skillRow = Instantiate(SkillRow, ScrollContent.transform);

                skillRow.GetComponent<CharacterPassiveRow>().UpdatePanel(state, passive);

                yield return new WaitForFixedUpdate();
            }
        }
    }
}