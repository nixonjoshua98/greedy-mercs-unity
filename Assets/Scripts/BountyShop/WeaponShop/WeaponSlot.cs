using UnityEngine;
using UnityEngine.UI;


namespace BountyUI
{
    using WeaponData;

    public class WeaponSlot : MonoBehaviour
    {
        public Text ownedText;

        public Image icon;
        public Image iconBorder;

        public Button button;

        ScriptableCharacter character;

        int weaponIndex;

        public void Init(ScriptableCharacter _character, int _weaponIndex /* Position in ScriptableCharacter.weapons (WeaponTier - 1)*/)
        {
            character = _character;
            weaponIndex = _weaponIndex;

            ScriptableWeapon weapon = character.weapons[weaponIndex];

            icon.sprite = weapon.icon;
        }

        public void FixedUpdate()
        {
            WeaponStaticData staticData = StaticData.Weapons.Get(weaponIndex);

            int weaponOwned = GameState.Weapons.Get(character.character, weaponIndex);

            ownedText.text = weaponOwned.ToString();

            iconBorder.color = weaponOwned >= staticData.maxOwned ? Color.yellow : Color.white;
        }
    }
}