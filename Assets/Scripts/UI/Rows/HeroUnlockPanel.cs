using UnityEngine;
using UnityEngine.UI;

public class HeroUnlockPanel : MonoBehaviour
{
    [SerializeField] Text TitleText;
    [SerializeField] Text CostText;

    void Start()
    {
        UpdatePanel();
    }

    void UpdatePanel()
    {
        if (CharacterResources.GetNextHero(out ScriptableCharacter chara))
        {
            TitleText.text = chara.name;

            CostText.text = Utils.Format.FormatNumber(chara.purchaseCost);
        }

        else
            Destroy(gameObject);
    }

    // === Button Callbacks ===

    public void OnUnlockButton()
    {
        if (CharacterResources.GetNextHero(out ScriptableCharacter chara))
        {
            if (GameState.Player.gold >= chara.purchaseCost)
            {
                GameState.Player.gold -= chara.purchaseCost;

                GameState.Characters.AddHero(chara.character);

                EventManager.OnHeroUnlocked.Invoke(chara.character);
            }

            UpdatePanel();
        }
    }
}
