using UnityEngine;
using UnityEngine.UI;


namespace GreedyMercs
{
    public class WeaponSlot : MonoBehaviour
    {
        public Text ownedText;

        public Image icon;
        public Image iconBorder;

        public Button button;

        CharacterSO character;

        int weaponIndex;

        public void Init(CharacterSO _character, int _weaponIndex)
        {
            character   = _character;
            weaponIndex = _weaponIndex;

            WeaponSO weapon = character.weapons[weaponIndex];

            Utils.UI.SetImageScaleW(icon, weapon.icon, 150.0f);

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