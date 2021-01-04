using System;

using UnityEngine;
using UnityEngine.UI;

using WeaponStaticData = WeaponData.WeaponStaticData;

public class WeaponBuyConfirm : MonoBehaviour
{
    [SerializeField] Slider slider;

    [Header("Text")]
    [SerializeField] Text costText;
    [SerializeField] Text titleText;
    [SerializeField] Text descText;
    [SerializeField] Text sliderText;
    [Space]
    [SerializeField] Button okButton;

    Action<bool, int> callback;

    WeaponStaticData weaponStaticData;

    public void Init(ScriptableCharacter chara, WeaponStaticData weapon, int maxVal, Action<bool, int> func)
    {
        weaponStaticData    = weapon;
        callback            = func;

        titleText.text  = chara.name;
        descText.text   = string.Format("Tier {0} Weapon", weapon.tier);

        slider.maxValue = maxVal;
    }

    public void OnSliderValueChanged()
    {
        int cost = weaponStaticData.cost * (int)slider.value;

        sliderText.text = slider.value.ToString();
        costText.text   = cost + "x Bounty Points";

        okButton.interactable = GameState.Player.bountyPoints >= cost;

        if (cost > GameState.Player.bountyPoints)
            costText.text = "<color=red>" + costText.text + "</color>";
    }

    public void OnClick(int index)
    {
        bool confirmed = index == 0;

        callback(confirmed, (int)slider.value);

        Destroy(gameObject);
    }
}
