using System;

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
        void OnWeaponSelected(int weaponTier)
        {
            int weaponOwned = GameState.Weapons.Get(character.character, weaponTier);

            WeaponStaticData staticWeaponData = StaticData.Weapons.Get(weaponTier);

            // Already own max amount
            if (weaponOwned >= staticWeaponData.maxOwned)
                return;

            // Local callback function
            void callback(bool confirmed, int total) => OnConfirm(weaponTier, confirmed, total);

            // The number of weapons the user can buy (this does not factor in if they can afford it)
            int maxLevelsBuy = Mathf.Max(0, staticWeaponData.maxOwned - weaponOwned);

            WeaponBuyConfirm confirm = Utils.UI.Instantiate(ConfirmSliderObject, Vector3.zero).GetComponent<WeaponBuyConfirm>();

            // Init the confirm widget
            confirm.Init(character, staticWeaponData, maxLevelsBuy, callback);
        }

        void OnConfirm(int weaponTier, bool confirmed, int total)
        {
            // Local method to forward some arguments to the method
            void ServerCallback(long code, string compressed) => OnServerWeaponBuy(weaponTier, total, code, compressed);

            if (confirmed && total > 0)
            {
                WeaponStaticData weapon = StaticData.Weapons.Get(weaponTier);

                int cost = weapon.cost * total;

                // User can afford (lcoally) so we ask the server to verify
                if (cost <= GameState.Player.bountyPoints)
                {
                    var node = Utils.Json.GetDeviceNode();

                    node.Add("characterId", (int)character.character);
                    node.Add("weaponId", weaponTier);
                    node.Add("buying", total);

                    Server.BuyWeapon(this, ServerCallback, node);
                }
            }
        }

        void OnServerWeaponBuy(int weaponIndex, int total, long code, string compressed)
        {
            if (code == 200)
            {
                WeaponStaticData weapon = StaticData.Weapons.Get(weaponIndex);

                // Subtract the amount
                GameState.Player.bountyPoints -= (weapon.cost * total);

                // Add the weapons to the user
                GameState.Weapons.Add(character.character, weaponIndex, total);

                EventManager.OnWeaponBought.Invoke(character, weaponIndex);
            }
        }
    }
}