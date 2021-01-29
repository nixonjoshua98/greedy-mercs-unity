using System.Collections;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs.Skills.UI
{
    public abstract class SkillButton : MonoBehaviour
    {
        protected SkillID skill;

        public void OnClick()
        {
            SkillState state = GameState.Skills.Get(skill);

            if (!state.IsActive)
            {
                SkillLevel levelData = state.LevelData;

                if (GameState.Player.currentEnergy >= levelData.EnergyCost)
                {
                    GameState.Player.currentEnergy -= levelData.EnergyCost;

                    GameState.Skills.ActivateSkill(skill);

                    Events.OnSkillActivated.Invoke();

                    Activate();
                }
            }
        }

        protected abstract void Activate();
    }
}