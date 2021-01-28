using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs.Armoury.UI
{
    using GreedyMercs.Armoury.Data;
    using SimpleJSON;

    public class ArmouryWeaponPanel : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] Button upgradeButton;
        [SerializeField] Text upgradeText;

        [Header("Components (Weapon)")]
        [SerializeField] Image colouredWeapon;
        [SerializeField] Image shadowWeapon;

        [Header("Components (Level + Damage)")]
        [SerializeField] Text levelText;
        [SerializeField] Text damageText;

        ArmouryItemSO weaponItem;

        public void Init(ArmouryItemSO item)
        {
            weaponItem = item;

            Utils.UI.ScaleImageW(colouredWeapon, item.icon, 400.0f);
            Utils.UI.ScaleImageW(shadowWeapon, item.icon, 400.0f);

            UpdateUI();
        }

        void UpdateUI()
        {
            string Stringify(double d) => Utils.Format.FormatNumber(d * 100) + "%";

            ArmouryItemSO data          = StaticData.Armoury.GetWeapon(weaponItem.Index);
            ArmouryWeaponState state    = GameState.Armoury.GetWeapon(weaponItem.Index);

            levelText.text  = string.Format("Level {0} -> {1}", state.level, state.level + 1);

            damageText.text = string.Format("{0} -> {1}", 
                Stringify(GameState.Armoury.DamageBonus(weaponItem.Index)),
                Stringify(GameState.Armoury.DamageBonus(weaponItem.Index, state.level + 1))
                );

            upgradeText.text = string.Format("Upgrade\n{0} Weapon Points", data.upgradeCost);

            upgradeButton.interactable = GameState.Player.weaponPoints >= data.upgradeCost;
        }

        public void OnClick()
        {
            ArmouryItemSO data = StaticData.Armoury.GetWeapon(weaponItem.Index);

            if (GameState.Player.weaponPoints >= data.upgradeCost)
            {
                JSONNode node = Utils.Json.GetDeviceNode();

                node["itemId"] = weaponItem.Index;

                Server.UpgradeArmouryItem(OnServerCallback, node);
            }


            UpdateUI();
        }

        void OnServerCallback(long code, string compressed)
        {
            if (code == 200)
            {
                JSONNode node = Utils.Json.Decompress(compressed);

                GameState.Armoury.UpgradeWeapon(weaponItem.Index, 1);

                GameState.Player.weaponPoints -= node["upgradeCost"].AsInt;
            }

            UpdateUI();
        }
    }
}
