using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillData
{
    [PreferBinarySerialization]
    [CreateAssetMenu(menuName = "Scriptables/Container/SkillList")]
    public class SkillListSO : ScriptableObject
    {
        public List<SkillSO> SkillList;

        Dictionary<SkillID, SkillSO> SkillDict;

        public void Init()
        {
            SkillDict = new Dictionary<SkillID, SkillSO>();

            foreach (SkillSO skill in SkillList)
            {
                SkillDict[skill.SkillID] = skill;
            }
        }

        public SkillSO Get(SkillID skill) => SkillDict[skill];
    }
}