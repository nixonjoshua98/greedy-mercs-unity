
using UnityEngine;

namespace GM
{
    public class CharacterPanel : MonoBehaviour
    {
        [SerializeField] CharacterPassivesPanel skillsPanel;

        public void SetHero(UnitID chara)
        {
            skillsPanel.SetHero(chara);
        }
    }
}