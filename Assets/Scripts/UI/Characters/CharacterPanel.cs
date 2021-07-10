
using UnityEngine;

namespace GM
{
    public class CharacterPanel : MonoBehaviour
    {
        [SerializeField] CharacterPassivesPanel skillsPanel;

        public void SetHero(MercID chara)
        {
            skillsPanel.SetHero(chara);
        }
    }
}