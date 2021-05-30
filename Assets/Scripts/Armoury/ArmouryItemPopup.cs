using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs.Armoury.UI
{
    using GM.Armoury;

    using GM.UI;

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

        ArmouryItemData serverItemData;


        public void Init(ArmouryItemData item)
        {
            serverItemData = item;

            colouredWeapon.sprite = shadowWeapon.sprite = serverItemData.Icon;

            stars.Show(serverItemData.Tier);

            UpdateUI();
        }


        void UpdateUI()
        {
            ArmouryItemState state  = ArmouryManager.Instance.GetItem(serverItemData.ID);

            string StringyLevelDamage(int lvl) => Utils.Format.FormatNumber(Formulas.Armoury.WeaponDamage(serverItemData.ID, lvl) * 100) + "%";
            string StringyEvoLevelDamage(int evo) => Utils.Format.FormatNumber(Formulas.Armoury.WeaponDamage(serverItemData.ID, state.level, evo) * 100) + "%";

            upgradeDamageText.text  = string.Format("{0} -> {1}", StringyLevelDamage(state.level), StringyLevelDamage(state.level + 1));
            evoDamageText.text      = string.Format("{0} -> {1}", StringyEvoLevelDamage(state.evoLevel), StringyEvoLevelDamage(state.evoLevel + 1));

            evolveSlider.maxValue = 5;
            evolveSlider.value      = state.owned;
        }


        void UpgradeItem()
        {
            ArmouryManager.Instance.UpgradeItem(serverItemData.ID, (code, body) => { UpdateUI(); });
        }


        public void EvolveItem()
        {
            ArmouryManager.Instance.EvolveItem(serverItemData.ID, (code, body) => { UpdateUI(); });
        }


        // = = = Button Callbacks = = =
        public void OnEvolveButton() { EvolveItem(); }
        public void OnUpgradeButton() { UpgradeItem(); }
        public void OnClosePopup() { Destroy(gameObject); }
    }
}
