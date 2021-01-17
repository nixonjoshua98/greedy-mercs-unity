
using UnityEngine;

namespace UI.Characters
{
    using CharacterData;

    public class CharacterPanel : MonoBehaviour
    {
        [SerializeField] CharacterPassivesPanel skillsPanel;

        public void SetHero(CharacterID hero)
        {
            skillsPanel.SetHero(hero);
        }

        public void OnClose()
        {
            Destroy(gameObject);
        }
    }
}