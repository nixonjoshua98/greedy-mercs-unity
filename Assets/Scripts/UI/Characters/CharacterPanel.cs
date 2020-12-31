
using UnityEngine;

using CharacterID = CharacterData.CharacterID;

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
