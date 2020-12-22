using UnityEngine;
using UnityEngine.UI;

public class HeroUnlockPanel : MonoBehaviour
{
    [SerializeField] Text Title;
    [Space]
    [SerializeField] GameObject ErrorMessageObject;


    void OnEnable()
    {
        UpdatePanel();
    }

    public void OnUnlockButton()
    {
        if (HeroResources.GetNextHeroUnlock(out int stage, out HeroID hero))
        {
            if (GameState.stage.stage > stage)
            {
                GameState.heroes.Add(new HeroState() { heroId = hero });
            }

            else
            {
                Utils.UI.ShowError(ErrorMessageObject, "Unlockable hero", "This hero can be unlocked after completing stage " + "<color=orange>" + stage.ToString() + "</color>");
            }

            UpdatePanel();
        }
    }

    void UpdatePanel()
    {
        if (HeroResources.GetNextHeroUnlock(out int stage, out var _))
        {
            Title.text = "Unlock after completing stage " + stage.ToString();
        }

        else
        {
            Destroy(gameObject);
        }
    }
}
