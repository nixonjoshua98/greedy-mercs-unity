
using UnityEngine;

namespace UI.Characters
{
    using Data.Characters;

    public class CharacterPanel : MonoBehaviour
    {
        [SerializeField] CharacterPassivesPanel skillsPanel;

        public void SetHero(CharacterID chara)
        {
            skillsPanel.SetHero(chara);
        }

        public void OnClose()
        {
            Destroy(gameObject);
        }
    }
}