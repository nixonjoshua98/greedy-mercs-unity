using System;

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
    [SerializeField] GameObject ConfirmSliderObject;

    List<WeaponSlot> weaponSlots;

    public void SetCharacter(ScriptableCharacter chara)
    {
        character = chara;

        characterIcon.sprite = chara.icon;

        StartCoroutine(Create());
    }

    IEnumerator Create()
    {
        weaponSlots = new List<WeaponSlot>();

        for (int i = 0; i < character.weapons.Length; ++i)
        {
            int temp = i;

            ScriptableWeapon weapon = character.weapons[i];

            GameObject spawned = Instantiate(CharaWeaponSlotObject, weaponsScrollContent);

            WeaponSlot slot = spawned.GetComponent<WeaponSlot>();
           
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
    void OnWeaponSelected(int weaponTier)
    {
        int weaponOwned = GameState.Weapons.Get(character.character, weaponTier);

        var staticWeaponData = StaticData.Weapons.Get(weaponTier);

        // Already own max amount
        if (weaponOwned >= staticWeaponData.maxOwned)
        {
            return;
        }

        void callback(bool confirmed, int total)
        {
            OnConfirm(weaponTier, confirmed, total);
        }

        int maxLevelsBuy = Mathf.Max(0, staticWeaponData.maxOwned - weaponOwned);

        WeaponBuyConfirm compo = Utils.UI.Instantiate(ConfirmSliderObject, Vector3.zero).GetComponent<WeaponBuyConfirm>();

        compo.Init(character, staticWeaponData, maxLevelsBuy, callback);
    }

    void OnConfirm(int weaponTier, bool confirmed, int total)
    {
        void ServerCallback(long code, string compressed)
        {
            OnServerWeaponBuy(weaponTier, total, code, compressed);
        }

        if (confirmed && total > 0)
        {
            var weapon = StaticData.Weapons.Get(weaponTier);

            int cost = weapon.cost * total;

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

    void OnServerWeaponBuy(int weaponTier, int total, long code, string compressed)
    {
        if (code == 200)
        {
            var weapon = StaticData.Weapons.Get(weaponTier);

            GameState.Player.bountyPoints -= (weapon.cost * total);

            GameState.Weapons.Add(character.character, weaponTier, total);

            int highestWeapon = GameState.Weapons.GetHighestTier(character.character);

            if (weaponTier > highestWeapon)
                EventManager.OnCharacterWeaponChange.Invoke(character, weaponTier);
        }
    }
}