using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using SimpleJSON;

namespace GreedyMercs.Armoury.UI
{
    using GreedyMercs.UI;

    using GreedyMercs.Armoury.Data;

    public class ArmouryItemPopup : MonoBehaviour
    {
        [Header("Upgrade Components")]
        [SerializeField] Text upgradeDamageText;

        [Header("Evolve Components")]
        [SerializeField] Text evoDamageText;
        [SerializeField] Slider evolveSlider;

        [Header("Item Components")]
        [SerializeField] Image colouredWeapon;
        [SerializeField] Image shadowWeapon;

        [Header("References")]
        [SerializeField] StarRatingController stars;

        ArmouryItemSO armouryItem;

        public void Init(ArmouryItemSO item)
        {
            armouryItem = item;

            colouredWeapon.sprite   = item.icon;
            shadowWeapon.sprite     = item.icon;
        }

        void FixedUpdate()
        {
            if (armouryItem)
            {
                UpdateUI();
            }
        }

        void UpdateUI()
        {
            ArmouryItemState state    = GameState.Armoury.GetItem(armouryItem.ItemID);
            ArmouryItemSO data          = StaticData.Armoury.GetWeapon(armouryItem.ItemID);

            string StringyLevelDamage(int lvl) => Utils.Format.FormatNumber(Formulas.Armoury.WeaponDamage(armouryItem.ItemID, lvl) * 100) + "%";
            string StringyEvoLevelDamage(int evo) => Utils.Format.FormatNumber(Formulas.Armoury.WeaponDamage(armouryItem.ItemID, state.level, evo) * 100) + "%";

            upgradeDamageText.text  = string.Format("{0} -> {1}", StringyLevelDamage(state.level), StringyLevelDamage(state.level + 1));
            evoDamageText.text      = string.Format("{0} -> {1}", StringyEvoLevelDamage(state.evoLevel), StringyEvoLevelDamage(state.evoLevel + 1));

            evolveSlider.maxValue   = data.evoUpgradeCost;
            evolveSlider.value      = state.owned;

            stars.SetRating(armouryItem.starRating);
        }

        void UpgradeItem()
        {
            JSONNode node = Utils.Json.GetDeviceNode();

            node["itemId"] = armouryItem.ItemID;

            Server.Put("armoury", "upgradeItem", node, OnUpgradeCallback);
        }

        void OnUpgradeCallback(long code, string body)
        {
            if (code == 200)
            {
                JSONNode node = Utils.Json.Decompress(body);

                int levelsGained = node.GetValueOrDefault("levelsGained", 0);

                GameState.Armoury.UpgradeItem(armouryItem.ItemID, levelsGained);
            }
        }


        public void EvolveItem()
        {
            JSONNode node = Utils.Json.GetDeviceNode();

            node["itemId"] = armouryItem.ItemID;

            Server.Put("armoury", "evolveItem", node, OnEvolveCallback);
        }

        void OnEvolveCallback(long code, string compressed)
        {
            if (code == 200)
            {
                JSONNode node = Utils.Json.Decompress(compressed);

                int evoLevelsGained = node.GetValueOrDefault("evoLevelsGained", 0);

                GameState.Armoury.EvoUpgradeItem(armouryItem.ItemID, evoLevelsGained);
            }
        }

        // = = = Button Callbacks = = =
        public void OnUpgradeButton()
        {
            UpgradeItem();
        }

        public void OnEvolveButton()
        {

        }

        public void OnClosePopup()
        {
            Destroy(gameObject);
        }
    }
}
