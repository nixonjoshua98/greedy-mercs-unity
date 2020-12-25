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
            if (GameState.Player.gold >= hero.PurchaseCost)
            {
                GameState.Player.gold -= hero.PurchaseCost;

                GameState.Heroes.Add(new HeroState() { heroId = hero.HeroID });

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

            CostText.text = Utils.Format.FormatNumber(hero.PurchaseCost);
        }

        else
        {
            Destroy(gameObject);
        }
    }
}
