﻿using System.Collections;

using UnityEngine;
using UnityEngine.UI;

namespace GM.Skills.UI
{
    public abstract class SkillButton : MonoBehaviour
    {
        protected SkillID skill;

        public void OnClick()
        {
            SkillState state = SkillsManager.Instance.Get(skill);

            if (!state.IsActive)
            {
                SkillLevel levelData = state.LevelData;

                if (GameState.Player.currentEnergy >= levelData.EnergyCost)
                {
                    GameState.Player.currentEnergy -= levelData.EnergyCost;

                    SkillsManager.Instance.ActivateSkill(skill);

                    GlobalEvents.OnSkillActivated.Invoke();

                    Activate();
                }
            }
        }

        protected abstract void Activate();
    }
}