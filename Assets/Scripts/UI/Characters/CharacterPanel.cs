
using UnityEngine;

public class CharacterPanel : MonoBehaviour
{
    [SerializeField] CharacterSkillsPanel skillsPanel;

    public void SetHero(CharacterID hero)
    {
        skillsPanel.SetHero(hero);
    }

    public void OnClose()
    {
        Destroy(gameObject);
    }
}
