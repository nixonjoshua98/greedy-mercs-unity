using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class MeleeHeroAttack : HeroAttack
{
    [Header("Character Slots")]
    [SerializeField] GameObject weaponSlot;

    void Start()
    {
        int highestWeapon = GameState.Weapons.GetHighestTier(heroId);

        if (highestWeapon > 0)
        {
            ScriptableCharacter chara = CharacterResources.Instance.GetCharacter(heroId);

            SwapWeapons(chara.weapons[highestWeapon]);
        }

        EventManager.OnWeaponBought.AddListener(OnWeaponBought);
    }

    public override void OnAttackAnimationEnd()
    {
        DealDamage();
    }

    void OnWeaponBought(ScriptableCharacter chara, int weaponIndex)
    {
        if (chara.character == heroId)
        {
            ScriptableWeapon weapon = chara.weapons[weaponIndex];

            int highestWeapon = GameState.Weapons.GetHighestTier(chara.character);

            if (weaponIndex >= highestWeapon)
                SwapWeapons(weapon);
        }
    }

    void SwapWeapons(ScriptableWeapon weapon)
    {
        weaponSlot.GetComponent<SpriteRenderer>().sprite = weapon.icon;
    }
}