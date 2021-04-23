using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GreedyMercs.Skills.UI
{
    public class AutoClickRow : SkillRow
    {

        protected override void OnEnable() => InvokeRepeating("UpdateUI", 0.0f, 0.5f);

        void OnDisable() => CancelInvoke("UpdateUI");

        protected override string GetDescription()
        {
            if (!GameState.Skills.IsUnlocked(SkillID))
                return base.GetDescription();

            string effect   = Utils.Format.FormatNumber(StatsCache.Skills.AutoClickDamage());
            double duration = StatsCache.Skills.SkillDuration(SkillID);

            return string.Format("Duration <color=orange>{0}s</color>\nDeal <color=orange>{1}</color> damage every <color=orange>0.1</color> seconds", duration, effect);
        }
    }
}