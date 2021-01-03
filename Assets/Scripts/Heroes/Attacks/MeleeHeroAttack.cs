using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class MeleeHeroAttack : HeroAttack
{
    [Header("Character Slots")]
    [SerializeField] GameObject weaponSlot;

    void Start()
    {
        EventManager.OnCharacterWeaponChange.AddListener(OnCharacterWeaponChange);
    }

    public override void OnAttackAnimationEnd()
    {
        DealDamage();
    }

    void OnCharacterWeaponChange(ScriptableCharacter chara, int index)
    {
        if (chara.character == heroId)
        {
            ScriptableWeapon weapon = chara.weapons[index];

            SwapWeapons(weapon.prefab);
        }
    }

    void SwapWeapons(GameObject newWeapon)
    {
        Instantiate(newWeapon, weaponSlot.transform);

        Destroy(weaponSlot.transform.GetChild(0).gameObject);
    }
}
