using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class CharacterWeaponsRow : MonoBehaviour
{
    ScriptableCharacter character;

    [Header("Components")]
    [SerializeField] Image characterIcon;

    [Header("Transforms")]
    [SerializeField] Transform weaponsScrollContent;

    [Header("Prefabs")]
    [SerializeField] GameObject CharaWeaponSlotObject;

    List<CharaterWeaponShopSlot> weaponSlots;

    public void SetCharacter(ScriptableCharacter chara)
    {
        character = chara;

        characterIcon.sprite = chara.icon;
    }

    IEnumerator Start()
    {
        SetCharacter(CharacterResources.Instance.GetCharacter(CharacterData.CharacterID.GOLEM));

        weaponSlots = new List<CharaterWeaponShopSlot>();

        for (int i = 0; i < character.weapons.Length; ++i)
        {
            int temp = i;

            ScriptableWeapon weapon = character.weapons[i];

            GameObject spawned = Instantiate(CharaWeaponSlotObject, weaponsScrollContent);

            CharaterWeaponShopSlot slot = spawned.GetComponent<CharaterWeaponShopSlot>();

            slot.icon.sprite = weapon.icon;

            slot.button.onClick.AddListener(delegate { OnWeaponSelected(temp); });

            weaponSlots.Add(slot);

            yield return new WaitForFixedUpdate();
        }
    }

    void FixedUpdate()
    {
        for (int i = 0; i < weaponSlots.Count; ++i)
        {
            int weaponOwned = GameState.Weapons.Get(character.character, i);

            weaponSlots[i].SetWeaponsOwned(weaponOwned);
        }
    }

    // === Button Callbacks ===
    void OnWeaponSelected(int index)
    {
        int weaponOwned = GameState.Weapons.Get(character.character, index);

        GameState.Weapons.Add(character.character, index, 1);

        EventManager.OnCharacterWeaponChange.Invoke(character, index);
    }
}
