using System;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace WeaponsUI
{
    using WeaponData;
    using CharacterData;

    public class WeaponSelection : MonoBehaviour
    {
        CharacterSO character;

        [Header("Components")]
        [SerializeField] Image characterIcon;

        [Header("Transforms")]
        [SerializeField] Transform weaponsScrollContent;

        [Header("Prefabs")]
        [SerializeField] GameObject CharaWeaponSlotObject;
        [SerializeField] GameObject ConfirmSliderObject;

        List<WeaponSlot> weaponSlots;

        void Awake()
        {
            weaponSlots = new List<WeaponSlot>();

            SetCharacter(StaticData.Chars.CharacterList[0]);
        }

        public void SetCharacter(CharacterSO chara)
        {
            character = chara;

            characterIcon.sprite = chara.icon;

            Create();
        }

        void Create()
        {
            foreach (WeaponSlot slot in weaponSlots)
                Destroy(slot.gameObject);

            weaponSlots = new List<WeaponSlot>();

            for (int i = 0; i < character.weapons.Length; ++i)
            {
                int temp = i;

                WeaponSO weapon = character.weapons[i];

                GameObject spawned = Instantiate(CharaWeaponSlotObject, weaponsScrollContent);
                
                WeaponSlot slot = spawned.GetComponent<WeaponSlot>();

                slot.Init(character, i);

                slot.button.onClick.AddListener(delegate { OnWeaponSelected(temp); });

                weaponSlots.Add(slot);
            }
        }
        // === Button Callbacks ===
        void OnWeaponSelected(int weaponIndex)
        {
            int weaponOwned = GameState.Weapons.Get(character.CharacterID, weaponIndex);

            WeaponStaticData staticWeaponData = StaticData.Weapons.GetWeaponAtIndex(weaponIndex);

            // Local callback function
            void callback(int buying, Action panelCallback) => OnConfirm(weaponIndex, buying, panelCallback);

            WeaponInfoPanel confirm = Utils.UI.Instantiate(ConfirmSliderObject, Vector3.zero).GetComponent<WeaponInfoPanel>();

            // Init the confirm widget
            confirm.Init(character, character.weapons[weaponIndex], weaponIndex, callback);
        }

        void OnConfirm(int weaponIndex, int buying, Action callback)
        {
            // Local method to forward some arguments to the method
            void ServerCallback(long code, string compressed) => OnServerWeaponBuy(weaponIndex, code, compressed, callback);

            WeaponStaticData weapon = StaticData.Weapons.GetWeaponAtIndex(weaponIndex);

            // User can afford (lcoally) so we ask the server to verify
            if (weapon.buyCost <= GameState.Player.bountyPoints)
            {
                var node = Utils.Json.GetDeviceNode();

                node.Add("characterId", (int)character.CharacterID);
                node.Add("weaponId", weaponIndex);
                node.Add("buyAmount", buying);

                Server.BuyWeapon(this, ServerCallback, node);
            }
        }

        void OnServerWeaponBuy(int weaponIndex, long code, string compressed, Action callback)
        {
            if (code == 200)
            {
                var node = Utils.Json.Decompress(compressed);
              
                Events.OnWeaponBought.Invoke(character, weaponIndex);

                GameState.Update(node);
            }

            callback();
        }
    }
}