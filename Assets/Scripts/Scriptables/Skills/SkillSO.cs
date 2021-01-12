using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace SkillData
{    public enum SkillID
    {
        GOLD_RUSH = 0,
    }

    [PreferBinarySerialization]
    [CreateAssetMenu(menuName = "Scriptables/Skill")]
    public class SkillSO : ScriptableObject
    {
        public SkillID SkillID;

        public float Duration;
    }
}