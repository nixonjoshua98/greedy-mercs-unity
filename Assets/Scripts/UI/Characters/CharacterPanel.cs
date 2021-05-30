
using UnityEngine;

namespace GreedyMercs
{
    public class CharacterPanel : MonoBehaviour
    {
        [SerializeField] CharacterPassivesPanel skillsPanel;

        public void SetHero(CharacterID chara)
        {
            skillsPanel.SetHero(chara);
        }
    }
}