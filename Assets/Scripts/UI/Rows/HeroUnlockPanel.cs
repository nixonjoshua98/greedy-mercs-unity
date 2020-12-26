using UnityEngine;
using UnityEngine.UI;

public class HeroUnlockPanel : MonoBehaviour
{
    [SerializeField] Text TitleText;
    [SerializeField] Text CostText;


    void OnEnable()
    {
        UpdatePanel();
    }

    public void OnUnlockButton()
    {
        if (CharacterResources.GetNextHero(out CharacterStaticData hero))
        {
            if (GameState.Player.gold >= hero.PurchaseCost)
            {
                GameState.Player.gold -= hero.PurchaseCost;

                GameState.Characters.AddHero(hero.HeroID);

                EventManager.OnHeroUnlocked.Invoke(hero.HeroID);
            }

            UpdatePanel();
        }
    }

    void UpdatePanel()
    {
        if (CharacterResources.GetNextHero(out CharacterStaticData hero))
        {
            TitleText.text = hero.Name;

            CostText.text = Utils.Format.FormatNumber(hero.PurchaseCost);
        }

        else
        {
            Destroy(gameObject);
        }
    }
}
