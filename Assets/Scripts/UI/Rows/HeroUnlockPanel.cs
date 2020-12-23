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
        if (HeroResources.GetNextHero(out HeroStaticData hero))
        {
            if (GameState.player.gold >= hero.PurchaseCost)
            {
                GameState.player.gold -= hero.PurchaseCost;

                GameState.heroes.Add(new HeroState() { heroId = hero.HeroID });

                EventManager.OnHeroUnlocked.Invoke(hero.HeroID);
            }

            UpdatePanel();
        }
    }

    void UpdatePanel()
    {
        if (HeroResources.GetNextHero(out HeroStaticData hero))
        {
            TitleText.text = hero.Name;

            CostText.text = Utils.Format.DoubleToString(hero.PurchaseCost);
        }

        else
        {
            Destroy(gameObject);
        }
    }
}
