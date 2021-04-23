using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using SimpleJSON;

namespace GreedyMercs.Armoury.UI
{
    using GreedyMercs.Armoury.Data;

    public class ArmouryWeaponPanel : MonoBehaviour
    {
        [Header("Upgrade Components")]
        [SerializeField] Text upgradeDamageText;
        [SerializeField] Button upgradeButton;

        [Header("Evolve Components")]
        [SerializeField] Text evoDamageText;
        [SerializeField] Slider evolveSlider;
        [SerializeField] Button evoUpgradeButton;

        [Header("Item Components")]
        [SerializeField] Image colouredWeapon;
        [SerializeField] Image shadowWeapon;

        [Header("References")]
        [SerializeField] GameObject[] stars;

        ArmouryItemSO armouryItem;

        public void Init(ArmouryItemSO item)
        {
            armouryItem = item;

            colouredWeapon.sprite   = item.icon;
            shadowWeapon.sprite     = item.icon;

            UpdateUI();
        }

        void UpdateUI()
        {
            ArmouryWeaponState state    = GameState.Armoury.GetWeapon(armouryItem.ItemID);
            ArmouryItemSO data          = StaticData.Armoury.GetWeapon(armouryItem.ItemID);

            string StringyLevelDamage(int lvl) => Utils.Format.FormatNumber(Formulas.Armoury.WeaponDamage(armouryItem.ItemID, lvl) * 100) + "%";
            string StringyEvoLevelDamage(int evo) => Utils.Format.FormatNumber(Formulas.Armoury.WeaponDamage(armouryItem.ItemID, state.level, evo) * 100) + "%";

            upgradeDamageText.text  = string.Format("{0} -> {1}", StringyLevelDamage(state.level), StringyLevelDamage(state.level + 1));
            evoDamageText.text      = string.Format("{0} -> {1}", StringyEvoLevelDamage(state.evoLevel), StringyEvoLevelDamage(state.evoLevel + 1));

            upgradeButton.interactable      = GameState.Inventory.armouryPoints >= data.upgradeCost;
            evoUpgradeButton.interactable   = state.owned >= data.evoUpgradeCost;

            evolveSlider.maxValue   = data.evoUpgradeCost;
            evolveSlider.value      = state.owned;

            UpdateStarRating();
        }

        void UpdateStarRating()
        {
            for (int i = 0; i < stars.Length; ++i)
            {
                stars[i].SetActive(i <= armouryItem.starRating - 1);
            }
        }

        // === Callbacks ===

        public void UpgradeItem()
        {
            ArmouryItemSO data = StaticData.Armoury.GetWeapon(armouryItem.ItemID);

            if (GameState.Inventory.armouryPoints >= data.upgradeCost)
            {
                JSONNode node = Utils.Json.GetDeviceNode();

                node["itemId"] = armouryItem.ItemID;

                Server.UpgradeArmouryItem(OnUpgradeCallback, node);
            }

            UpdateUI();
        }

        public void EvolveItem()
        {
            ArmouryItemSO data          = StaticData.Armoury.GetWeapon(armouryItem.ItemID);
            ArmouryWeaponState state    = GameState.Armoury.GetWeapon(armouryItem.ItemID);

            if (state.owned >= data.evoUpgradeCost)
            {
                JSONNode node = Utils.Json.GetDeviceNode();

                node["itemId"] = armouryItem.ItemID;

                Server.EvolveArmouryItem(OnEvolveCallback, node);
            }

            UpdateUI();
        }

        void OnUpgradeCallback(long code, string compressed)
        {
            if (code == 200)
            {
                JSONNode node = Utils.Json.Decompress(compressed);

                GameState.Armoury.UpgradeWeapon(armouryItem.ItemID, 1);

                GameState.Inventory.armouryPoints -= node["upgradeCost"].AsInt;
            }

            UpdateUI();
        }

        void OnEvolveCallback(long code, string compressed)
        {
            if (code == 200)
            {
                JSONNode node = Utils.Json.Decompress(compressed);

                ArmouryWeaponState state = GameState.Armoury.GetWeapon(armouryItem.ItemID);

                state.evoLevel += 1;

                state.owned = node["owned"].AsInt;
            }

            UpdateUI();
        }
    }
}
