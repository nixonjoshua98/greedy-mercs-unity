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

    public void SetCharacter(ScriptableCharacter chara)
    {
        character = chara;

        characterIcon.sprite = chara.icon;
    }

    IEnumerator Start()
    {
        SetCharacter(CharacterResources.Instance.GetCharacter(CharacterData.CharacterID.GOLEM));

        for (int i = 0; i < character.weapons.Length; ++i)
        {
            int temp = i;

            ScriptableWeapon weapon = character.weapons[i];

            GameObject spawned = Instantiate(CharaWeaponSlotObject, weaponsScrollContent);

            CharaterWeaponShopSlot slot = spawned.GetComponent<CharaterWeaponShopSlot>();

            slot.icon.sprite = weapon.icon;

            slot.button.onClick.AddListener(delegate { OnWeaponSelected(temp); });

            yield return new WaitForFixedUpdate();
        }
    }

    void OnWeaponSelected(int index)
    {
        EventManager.OnCharacterWeaponChange.Invoke(character, index);
    }
}
