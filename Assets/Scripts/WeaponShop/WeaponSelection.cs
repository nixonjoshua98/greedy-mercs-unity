﻿using System;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace WeaponsUI
{
    using WeaponData;

    public class WeaponSelection : MonoBehaviour
    {
        ScriptableCharacter character;

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

            SetCharacter(CharacterResources.Instance.Characters[0]);
        }

        public void SetCharacter(ScriptableCharacter chara)
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

                ScriptableWeapon weapon = character.weapons[i];

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
            int weaponOwned = GameState.Weapons.Get(character.character, weaponIndex);

            WeaponStaticData staticWeaponData = StaticData.Weapons.GetWeaponAtIndex(weaponIndex);

            // Local callback function
            void callback(Action panelCallback) => OnConfirm(weaponIndex, panelCallback);

            WeaponInfoPanel confirm = Utils.UI.Instantiate(ConfirmSliderObject, Vector3.zero).GetComponent<WeaponInfoPanel>();

            // Init the confirm widget
            confirm.Init(character, character.weapons[weaponIndex], weaponIndex, callback);
        }

        void OnConfirm(int weaponIndex, Action callback)
        {
            // Local method to forward some arguments to the method
            void ServerCallback(long code, string compressed) => OnServerWeaponBuy(weaponIndex, code, compressed, callback);

            WeaponStaticData weapon = StaticData.Weapons.GetWeaponAtIndex(weaponIndex);

            // User can afford (lcoally) so we ask the server to verify
            if (weapon.buyCost <= GameState.Player.bountyPoints)
            {
                var node = Utils.Json.GetDeviceNode();

                node.Add("characterId", (int)character.character);
                node.Add("weaponId", weaponIndex);

                Server.BuyWeapon(this, ServerCallback, node);
            }
        }

        void OnServerWeaponBuy(int weaponIndex, long code, string compressed, Action callback)
        {
            if (code == 200)
            {
                var node = Utils.Json.Decode(compressed);
              
                Events.OnWeaponBought.Invoke(character, weaponIndex);

                GameState.Update(node);
            }

            callback();
        }
    }
}