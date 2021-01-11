using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace WeaponsUI
{
    using CharacterData;

    public class CharacterWeaponShopIcon : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] Image icon;
        
        public Button button;

        // ===
        CharacterSO character;

        public void SetCharacter(CharacterSO _character)
        {
            character = _character;

            icon.sprite = character.icon;
        }
    }
}