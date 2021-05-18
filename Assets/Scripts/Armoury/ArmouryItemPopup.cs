using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs.Armoury.UI
{
    using GM.Armoury;

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

            stars.SetRating(armouryItem.itemTier);
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
            ArmouryItemState state  = ArmouryManager.Instance.GetItem(armouryItem.ItemID);
            ArmouryItemSO data      = StaticData.Armoury.GetWeapon(armouryItem.ItemID);

            string StringyLevelDamage(int lvl) => Utils.Format.FormatNumber(Formulas.Armoury.WeaponDamage(armouryItem.ItemID, lvl) * 100) + "%";
            string StringyEvoLevelDamage(int evo) => Utils.Format.FormatNumber(Formulas.Armoury.WeaponDamage(armouryItem.ItemID, state.level, evo) * 100) + "%";

            upgradeDamageText.text  = string.Format("{0} -> {1}", StringyLevelDamage(state.level), StringyLevelDamage(state.level + 1));
            evoDamageText.text      = string.Format("{0} -> {1}", StringyEvoLevelDamage(state.evoLevel), StringyEvoLevelDamage(state.evoLevel + 1));

            evolveSlider.maxValue   = data.evoUpgradeCost;
            evolveSlider.value      = state.owned;
        }


        void UpgradeItem()
        {
            ArmouryManager.Instance.UpgradeItem(armouryItem.ItemID, (code, body) => { });
        }


        public void EvolveItem()
        {
            ArmouryManager.Instance.EvolveItem(armouryItem.ItemID, (code, body) => { });
        }


        // = = = Button Callbacks = = =
        public void OnEvolveButton() { EvolveItem(); }
        public void OnUpgradeButton() { UpgradeItem(); }
        public void OnClosePopup() { Destroy(gameObject); }
    }
}
