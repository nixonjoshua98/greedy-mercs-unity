using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace WeaponsUI
{
    public class CharacterWeaponShopIcon : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] Image icon;
        
        public Button button;

        // ===
        ScriptableCharacter character;

        public void SetCharacter(ScriptableCharacter _character)
        {
            character = _character;

            icon.sprite = character.icon;
        }
    }
}