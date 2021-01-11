using UnityEngine;
using UnityEngine.UI;


namespace WeaponsUI
{
    using WeaponData;
    using CharacterData;

    public class WeaponSlot : MonoBehaviour
    {
        public Text ownedText;

        public Image icon;
        public Image iconBorder;

        public Button button;

        CharacterSO character;

        int weaponIndex;

        public void Init(CharacterSO _character, int _weaponIndex /* Position in ScriptableCharacter.weapons (WeaponTier - 1)*/)
        {
            character = _character;
            weaponIndex = _weaponIndex;

            ScriptableWeapon weapon = character.weapons[weaponIndex];

            icon.sprite = weapon.icon;

            FixedUpdate();
        }

        public void FixedUpdate()
        {
            WeaponStaticData staticData = StaticData.Weapons.GetWeaponAtIndex(weaponIndex);

            int weaponOwned = GameState.Weapons.Get(character.CharacterID, weaponIndex);

            ownedText.text = weaponOwned.ToString();

            iconBorder.color = weaponOwned >= staticData.maxOwned ? Color.yellow : Color.white;
        }
    }
}