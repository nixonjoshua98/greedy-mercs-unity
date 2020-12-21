using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using SimpleJSON;

public class HeroUnlockPanel : MonoBehaviour
{
    [SerializeField] Text Title;
    [Space]
    [SerializeField] GameObject ErrorMessageObject;

    Dictionary<int, HeroID> HeroUnlocks = new Dictionary<int, HeroID>()
    {
        {1,     HeroID.WRAITH_LIGHTNING },
        {10,    HeroID.GOLEM_STONE },
        {35,    HeroID.FALLEN_ANGEL },
        {75,    HeroID.SATYR_FIRE },
    };


    void OnEnable()
    {
        UpdatePanel();
    }

    public void OnUnlockButton()
    {
        if (GetNextHeroUnlock(out int stage, out HeroID hero))
        {
            if (GameState.stage.stage >= stage)
            {
                GameState.heroes.Add(new HeroState() { heroId = hero });
            }

            else
            {
                Utils.UI.ShowError(ErrorMessageObject, "Stage Reward", "This hero can be unlocked after reaching stage " + "<color=\"orange\">" + stage.ToString() + "</color>");
            }

            UpdatePanel();
        }
    }

    void UpdatePanel()
    {
        if (GetNextHeroUnlock(out int stage, out var _))
        {
            Title.text = "Unlocks at stage " + stage.ToString();
        }

        else
        {
            Title.text = "Nothing left to unlock!";
        }
    }

    bool GetNextHeroUnlock(out int stage, out HeroID hero)
    {
        stage = 0;
        hero = HeroID.ERROR;

        List<int> values = HeroUnlocks.Keys.ToList();

        values.Sort();

        foreach (int stageUnlock in values)
        {
            if (!GameState.TryGetHeroState(HeroUnlocks[stageUnlock], out var _) || stageUnlock > GameState.stage.stage)
            {
                stage = stageUnlock;

                hero = HeroUnlocks[stageUnlock];

                return true;
            }
        }

        return false;
    }
}
