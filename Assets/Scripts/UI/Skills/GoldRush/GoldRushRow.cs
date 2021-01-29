using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GreedyMercs
{
    public class GoldRushRow : SkillRow
    {
        protected void Awake()
        {
            SkillID = SkillID.GOLD_RUSH;
        }
    }
}