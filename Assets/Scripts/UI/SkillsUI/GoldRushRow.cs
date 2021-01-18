using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GreedyMercs
{
    public class GoldRushRow : SkillRow
    {
        protected override string SkillDescription()
        {
            if (!GameState.Skills.IsUnlocked(SkillID))
                return "Locked";

            string effect   = Utils.Format.FormatNumber(StatsCache.GoldRushBonus() * 100);
            double duration = StatsCache.SkillDuration(SkillID);

            return string.Format("Lasts for <color=orange>{1}s</color>\nMultiply <color=orange>All Gold</color> by <color=orange>{0}%</color>", effect, duration);
        }
    }
}