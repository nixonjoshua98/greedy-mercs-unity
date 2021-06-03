
using UnityEngine;

namespace GM
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