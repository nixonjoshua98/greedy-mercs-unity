using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM.Armoury.UI
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
            // We use the manager a lot, so we cache it temporary
            ArmouryManager armoury = UserData.Get().Armoury;

            // Grab the current state
            ArmouryItemState state  = armoury.GetItem(serverItemData.ID);

            // Calculate the values we need
            double currentDamage    = armoury.WeaponDamage(serverItemData.ID);
            double nextLevelDamage  = armoury.WeaponDamage(serverItemData.ID, state.level + 1);
            double nextEvoDamage    = armoury.WeaponDamage(serverItemData.ID, state.level, state.evoLevel + 1);

            // Format the above values as strings
            string currentDmgString = FormatString.Number(currentDamage * 100, prefix: "%");
            string nextEvoDmgString = FormatString.Number(nextEvoDamage * 100, prefix: "%");
            string nextLvlDmgString = FormatString.Number(nextLevelDamage * 100, prefix: "%");

            // Set the text widgets
            upgradeDamageText.text  = string.Format("{0} -> {1}", currentDmgString, nextLvlDmgString);
            evoDamageText.text      = string.Format("{0} -> {1}", currentDmgString, nextEvoDmgString);

            // Update the evolve level slider
            evolveSlider.maxValue   = 5;
            evolveSlider.value      = state.owned;
        }


        // = = = Button Callbacks = = = //

        public void OnEvolveButton()
        {
            UserData.Get().Armoury.EvolveItem(serverItemData.ID, () => { UpdateUI(); });
        }


        public void OnUpgradeButton()
        {
            UserData.Get().Armoury.UpgradeItem(serverItemData.ID, () => { UpdateUI(); });
        }


        public void OnClosePopup() 
        { 
            Destroy(gameObject); 
        }
    }
}
