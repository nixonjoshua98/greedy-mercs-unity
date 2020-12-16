using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class HeroRow : MonoBehaviour
{
    HeroID assignedHeroId;

    // =====

    public Button SquadButton;
    public Text SquadButtonText;

    [Space]

    public Button UpgradeButton;
    public Text UpgradeButtonText;

    [Space]

    public Text DamageText;
    public Text NameText;

    [Space]

    public Image IconImage;

    // =====

    public void AssignHero(HeroID newHeroId)
    {
        assignedHeroId = newHeroId;
    }

    public void OnEnable()
    {

    }

    public void OnSquadButton()
    {

    }

    public void OnBuyButton()
    {

    }
}